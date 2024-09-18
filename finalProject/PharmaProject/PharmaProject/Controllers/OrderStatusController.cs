using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaProject.Models;
using System.Linq;
using System.Security.Claims;

namespace PharmaProject.Controllers
{
    public class OrderStatusController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderStatusController(ApplicationDbContext context)
        {
            _context = context;
        }



        [Authorize(Roles = "User, Admin")]
        public IActionResult Index()
        {
            // Retrieve the logged-in user's username
            var loggedInUsername = User.Identity.Name;

            // Fetch the user from the database using the username
            var user = _context.Clients.FirstOrDefault(u => u.Username == loggedInUsername);

            // Check if the user was found
            if (user != null)
            {
                // Check if the logged-in user is an Admin
                if (User.IsInRole("Admin"))
                {
                    // Admin role: show all statuses
                    var allStatuses = _context.status.ToList();
                    return View(allStatuses);
                }
                else
                {
                    // User role: show statuses filtered by UserId
                    var userStatuses = _context.status
                        .Where(s => s.UserId == user.Id)
                        .ToList();
                    return View(userStatuses);
                }
            }

            // Return an empty view if no user was found
            return View(new List<OrderStatus>());
        }





        [Authorize(Roles = "Admin")]
        public IActionResult Create(int orderId, int userId)
        {
            // Create a new OrderStatus instance and set the OrderId and UserId
            var orderStatus = new OrderStatus
            {
                OrderId = orderId,
                UserId = userId
            };

            return View(orderStatus);
        }



        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(OrderStatus orderStatus)
        {
            if (ModelState.IsValid)
            {
                // Ensure OrderId is provided and valid
                if (orderStatus.OrderId > 0)
                {
                    var order = _context.orders.FirstOrDefault(o => o.Id == orderStatus.OrderId);

                    if (order != null)
                    {
                        // Ensure that the UserId is correctly linked to the order
                        orderStatus.UserId = order.UserId;

                        // Add the new order status to the context and save changes
                        _context.status.Add(orderStatus);
                        _context.SaveChanges();


                        TempData["SuccessMessage"] = "Order Status added & Sent Successfully!";
                        return RedirectToAction("Create");
                    }

                    ModelState.AddModelError("", "Invalid Order ID.");
                }
                else
                {
                    ModelState.AddModelError("", "Order ID is required.");
                }
            }

            return View(orderStatus); // Return to the Create view if validation fails
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var stat = _context.status.FirstOrDefault(x => x.Id == id);
            if (stat == null)
            {
                return NotFound();
            }
            return View(stat);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(OrderStatus stat)
        {
            if (ModelState.IsValid)
            {
                _context.status.Update(stat);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(stat);
        }
    }
}




