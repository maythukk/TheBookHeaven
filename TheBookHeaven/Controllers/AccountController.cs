using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using TheBookHeaven;
using TheBookHeaven.Models;

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
        public async Task<IActionResult> Login(string Username, string Password, string Role)
        {
            // Find user by username and role
            var user = _context.Users.FirstOrDefault(u =>
                u.Username == Username &&
                u.Role == Role);

            if (user != null)
            {
                var hasher = new PasswordHasher<User>();

                // Check if password matches hashed password
                var result = hasher.VerifyHashedPassword(user, user.Password, Password);

                if (result == PasswordVerificationResult.Success)
                {
                    // Set session info
                    HttpContext.Session.SetString("Username", user.Username);
                    HttpContext.Session.SetString("Role", user.Role);
                    HttpContext.Session.SetInt32("UserId", user.Id); // Store user ID in session

                    // Create claims for the logged-in user
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),  // Set Username as the Identity Name
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // Add user ID claim
                new Claim(ClaimTypes.Role, user.Role),      // Set Role as the Identity Role
                new Claim("UserId", user.Id.ToString())     // Custom claim for user ID
            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    // Sign in the user (this persists the authentication)
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                    // Redirect by role
                    if (user.Role == "Admin")
                        return RedirectToAction("Dashboard", "Admin");
                    else
                        return RedirectToAction("Index", "Home");
                }
            }

            // Login failed
            ViewBag.Error = "Invalid username or password.";
            ViewBag.Role = Role;
            ViewBag.Title = $"{Role} Login";
            return View();
        }

        // Logout method
        [HttpGet, HttpPost]
        public async Task<IActionResult> Logout()
        {
            // Sign out from cookie authentication
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Clear all session data
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }

        // Show Register view for new customers
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Check if username or email already exists
            bool usernameExists = _context.Users.Any(u => u.Username == model.Username);
            bool emailExists = _context.Users.Any(u => u.Email == model.Email);

            if (usernameExists)
            {
                ViewBag.Error = "Username already exists.";
                return View(model);
            }

            if (emailExists)
            {
                ViewBag.Error = "Email already exists.";
                return View(model);
            }

            // Hash the password
            var hasher = new PasswordHasher<User>();
            model.Password = hasher.HashPassword(model, model.Password);

            // Set default role
            model.Role = "Customer";

            _context.Users.Add(model);
            _context.SaveChanges();

            TempData["Message"] = "Account created successfully. You can now log in.";
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

        // GET: ChangePassword
        public IActionResult ChangePassword()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Login");

            return View();
        }

        // POST: ChangePassword
        [HttpPost]
        public IActionResult ChangePassword(ChangePassword model)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username)) return RedirectToAction("Login");

            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null) return RedirectToAction("Login");

            // Use PasswordHasher to verify old password
            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user, user.Password, model.OldPassword);

            if (result == PasswordVerificationResult.Failed)
            {
                ViewBag.Error = "Old password is incorrect.";
                return View(model);
            }

            // Check if new passwords match
            if (model.NewPassword != model.ConfirmPassword)
            {
                ViewBag.Error = "New passwords do not match.";
                return View(model);
            }

            // Hash and update new password
            user.Password = hasher.HashPassword(user, model.NewPassword);
            _context.SaveChanges();

            ViewBag.Success = "Password changed successfully!";
            return RedirectToAction("Profile");
        }

        // POST: Update username
        [HttpPost]
        public IActionResult EditUsername(string username)
        {
            var currentUsername = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(currentUsername))
                return RedirectToAction("Login");

            var user = _context.Users.FirstOrDefault(u => u.Username == currentUsername);
            if (user == null)
                return RedirectToAction("Login");

            if (!string.IsNullOrWhiteSpace(username))
            {
                user.Username = username;
                _context.SaveChanges();

                // Update session with new username
                HttpContext.Session.SetString("Username", username);
            }

            return RedirectToAction("Profile");
        }
    }
}