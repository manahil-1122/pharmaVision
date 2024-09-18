using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmaProject.Models;
using System.Linq;

namespace PharmaProject.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var ord = _context.orders.ToList();
            return View(ord);
        }

        [Authorize(Roles = "User")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public IActionResult Create(Order odr)
        {
            if (ModelState.IsValid)
            {
                // Get the logged-in user's ID
                var loggedInUsername = User.Identity.Name;
                var user = _context.Clients.FirstOrDefault(u => u.Username == loggedInUsername);
                if (user != null)
                {
                    odr.UserId = user.Id; // Associate the order with the logged-in user's ID
                    _context.orders.Add(odr);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Order sent to pharmacist admin";
                    return RedirectToAction("Create");
                }
            }
            return View(odr);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var ordToRemove = _context.orders.FirstOrDefault(x => x.Id == id);
            if (ordToRemove != null)
            {
                // Find and delete related order statuses
                var relatedStatuses = _context.status.Where(x => x.OrderId == id);
                _context.status.RemoveRange(relatedStatuses);

                // Delete the order itself
                _context.orders.Remove(ordToRemove);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

    }

}
