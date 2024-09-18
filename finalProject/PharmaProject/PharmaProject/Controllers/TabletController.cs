using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaProject.Models;
using System.Linq;

namespace PharmaProject.Controllers
{
    public class TabletController : Controller
    {
        private readonly ApplicationDbContext _context;
        public TabletController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var meds = _context.Medicine.ToList();
            return View(meds);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(Tablets tabs)
        {
            if (ModelState.IsValid)
            {
                _context.Medicine.Add(tabs);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Information added successfully!";
                return RedirectToAction("Create");
            }
            return View(tabs); // Return the same view with the current model if validation fails
        }



        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var tabsToRemove = _context.Medicine.FirstOrDefault(x => x.Id == id);
            if (tabsToRemove != null)
            {
                _context.Medicine.Remove(tabsToRemove);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var product = _context.Medicine.FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Tablets product)
        {
            if (ModelState.IsValid)
            {
                _context.Medicine.Update(product);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }
    }
}
