using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace TheBookHeaven.Controllers
{
    public class AdminController : Controller
    {
        // GET Admin/Dashboard
        public IActionResult Dashboard()
        {
            // Check if the user is logged in as Admin
            var role = HttpContext.Session.GetString("Role");
            if (role != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Username = HttpContext.Session.GetString("Username");
            return View();
        }
    }
}