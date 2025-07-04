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

        // Handle Login POST request for Admin and Customer
        [HttpPost]
        public IActionResult Login(string Username, string Password, string Role)
        {
            var user = _context.Users.FirstOrDefault(u =>
                u.Username == Username &&
                u.Password == Password &&
                u.Role == Role);

            if (user != null)
            {
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Role", user.Role);

                if (user.Role == "Admin")
                    return RedirectToAction("Dashboard", "Admin");
                else // Customer
                    return RedirectToAction("Index", "Home");
            }

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
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // Handle POST Register
        [HttpPost]
        public IActionResult Register(User user)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Please fill all fields correctly.";
                return View(user);
            }

            var existingUser = _context.Users.FirstOrDefault(u => u.Username == user.Username);
            if (existingUser != null)
            {
                ViewBag.Error = "Username already exists. Please choose another.";
                return View(user);
            }

            user.Role = "Customer";

            _context.Users.Add(user);
            _context.SaveChanges();

            TempData["Message"] = "Account registered successfully. Please log in.";
            return RedirectToAction("Login");
        }

        public IActionResult Profile()
        {
            // Ensure user is logged in
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Login");

            // Get user from database
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            if (user == null)
                return RedirectToAction("Login");

            return View(user);
        }

    }
}
