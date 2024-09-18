using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaProject.Models;
using System.Linq;

namespace PharmaProject.Controllers
{
    public class LiquidController : Controller
    {
        private readonly ApplicationDbContext _context;
        public LiquidController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var liq = _context.Syringe.ToList();
            return View(liq);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(Liquid liqs)
        {
            if (ModelState.IsValid)
            {
                _context.Syringe.Add(liqs);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Information added successfully!";
                return RedirectToAction("Create");
            }
            return View(liqs); // Return the same view with the current model if validation fails
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var liqsToRemove = _context.Syringe.FirstOrDefault(x => x.Id == id);
            if (liqsToRemove != null)
            {
                _context.Syringe.Remove(liqsToRemove);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var inject = _context.Syringe.FirstOrDefault(x => x.Id == id);
            if (inject == null)
            {
                return NotFound();
            }
            return View(inject);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Liquid inject)
        {
            if (ModelState.IsValid)
            {
                _context.Syringe.Update(inject);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(inject);
        }
    }
}
