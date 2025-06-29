using Microsoft.AspNetCore.Mvc;

namespace TheBookHeaven.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // later if needed
        [HttpPost]
        public IActionResult Login(string Username, string Password)
        {
            // Placeholder login logic
            if (Username == "admin" && Password == "admin123")
            {
                // To do: Redirect to admin view/dashboard
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid username or password.";
            return View();
        }

        public IActionResult Register()
        {
            return View(); 
        }

        public IActionResult AdminLogin()
        {
            return RedirectToAction("Login"); 
        }

        public IActionResult EmployeeLogin()
        {
            return RedirectToAction("Login"); 
        }
    }
}
