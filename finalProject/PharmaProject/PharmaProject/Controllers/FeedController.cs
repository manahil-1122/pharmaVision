using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaProject.Models;
using System.Linq;

namespace PharmaProject.Controllers
{
    public class FeedController : Controller
    {
        private readonly ApplicationDbContext _context;
        public FeedController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var rev = _context.reviews.ToList();
            return View(rev);
        }

        [Authorize(Roles = "User")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public IActionResult Create(Feedback feeds)
        {
            if (ModelState.IsValid)
            {
                _context.reviews.Add(feeds);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Feedback successfully sent!";
                return RedirectToAction("Create");
            }
            return View(feeds); // return to the Create view if validation fails
        }
    }
}
