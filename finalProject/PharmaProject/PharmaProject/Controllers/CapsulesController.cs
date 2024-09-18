using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaProject.Models;
using System.Linq;

namespace PharmaProject.Controllers
{
    public class CapsulesController: Controller
    {
        private readonly ApplicationDbContext _context;
        public CapsulesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var caps = _context.Encap.ToList();
            return View(caps);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(Capsules drug)
        {
            if (ModelState.IsValid)
            {
                _context.Encap.Add(drug);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Capsule added successfully!";
                return RedirectToAction("Create"); // Redirect to the same action to show success message
            }
            return View(drug); // Return the view with the current model if validation fails
        }



        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var drugToRemove = _context.Encap.FirstOrDefault(x => x.Id == id);
            if (drugToRemove != null)
            {
                _context.Encap.Remove(drugToRemove);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var items = _context.Encap.FirstOrDefault(x => x.Id == id);
            if (items == null)
            {
                return NotFound();
            }
            return View(items);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Capsules items)
        {
            if (ModelState.IsValid)
            {
                _context.Encap.Update(items);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(items);
        }
    }
}
