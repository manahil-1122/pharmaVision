using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmaProject.Models;
using System.Security.Claims;

namespace PharmaProject.Controllers
{
    public class MainController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MainController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Signup(Users usr)
        {
            if (string.IsNullOrEmpty(usr.Role))
            {
                usr.Role = "User"; // Default role can be changed
            }

            if (string.IsNullOrEmpty(usr.CompanyName))
            {
                usr.CompanyName = "DefaultCompany"; // Set a default value if needed
            }

            _context.Clients.Add(usr);
            _context.SaveChanges();

            HttpContext.Session.SetString("IsUserSignedUp", "true");
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            bool UserSignup = HttpContext.Session.GetString("IsUserSignedUp") == "true";
            ViewBag.IsUserSignedUp = UserSignup;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Users lgn)
        {
            var existingUser = _context.Clients.FirstOrDefault(p => p.Username == lgn.Username && p.Password == lgn.Password);
            if (existingUser != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, lgn.Username),
                    
                    new Claim(ClaimTypes.Role, existingUser.Role)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authprop = new AuthenticationProperties
                {
                    IsPersistent = false,
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authprop);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid login attempt";
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Main");
        }

        [HttpGet]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Main", new { returnUrl });
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, provider);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return RedirectToAction("Login", "Main");
            }

            var info = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (info.Principal == null)
            {
                return RedirectToAction("Login", "Main");
            }

            var username = info.Principal.Identity.Name;

            // Check if user already exists
            var existingUser = _context.Clients.FirstOrDefault(p => p.Username == username);
            if (existingUser == null)
            {
                // Create a new user if not already in the database
                var newUser = new Users
                {
                    Username = username,
                    Role = "User", // Default role or customize as needed
                    CompanyName = "DefaultCompany", // Set a default value for CompanyName
                    Password = "" // Set a default value for Password if needed, or handle as per your requirement
                };
                _context.Clients.Add(newUser);
                await _context.SaveChangesAsync(); // Ensure async save to prevent blocking
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "User") // Default role or customize as needed
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties { IsPersistent = false });

            return RedirectToAction("Index", "Home");
        }
    }
}
