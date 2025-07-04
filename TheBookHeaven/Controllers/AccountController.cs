using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using TheBookHeaven.Models;
using TheBookHeaven;
using System.Linq;

namespace TheBookHeaven.Controllers
{
    public class AccountController : Controller
    {
        private readonly MyDbContext _context;

        public AccountController(MyDbContext context)
        {
            _context = context;
        }

        // Show Customer login view
        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.Role = "Customer";
            ViewBag.Title = "Customer Login";
            ViewBag.Success = TempData["Message"]; // success message after register
            return View();
        }

        // Show Admin login view
        [HttpGet]
        public IActionResult AdminLogin()
        {
            ViewBag.Role = "Admin";
            ViewBag.Title = "Admin Login";
            return View("Login");
        }

        // Show Employee login view
        [HttpGet]
        public IActionResult EmployeeLogin()
        {
            ViewBag.Role = "Employee";
            ViewBag.Title = "Employee Login";
            return View("Login");
        }

        // Handle Login POST request for all roles
        [HttpPost]
        public IActionResult Login(string Username, string Password, string Role)
        {
            // Check user from database
            var user = _context.Users.FirstOrDefault(u =>
                u.Username == Username &&
                u.Password == Password &&
                u.Role == Role);

            if (user != null)
            {
                // Valid login
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Role", user.Role);

                // Redirect based on role
                if (user.Role == "Admin")
                    return RedirectToAction("Dashboard", "Admin");
                else if (user.Role == "Employee")
                    return RedirectToAction("Inventory", "Employee");
                else
                    return RedirectToAction("Index", "Home");
            }

            // Login failed
            ViewBag.Error = "Invalid username or password.";
            ViewBag.Role = Role;
            ViewBag.Title = $"{Role} Login";
            return View();
        }


        // Logout and clear session
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // Show Register view for new customers
        

        // Handle POST Register
        

    }
}
