using Microsoft.AspNetCore.Mvc;

namespace PharmaProject.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        public  AdminController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var people = _context.Clients.Where(u => u.Role != "Admin").ToList();
            return View(people);
        }
    }
}
