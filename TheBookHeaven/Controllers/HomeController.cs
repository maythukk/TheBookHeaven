using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TheBookHeaven.Models;
using TheBookHeaven;
using System.Linq;
using System.Security.Claims;
using TheBookHeaven.Helpers;

namespace TheBookHeaven.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MyDbContext _context;

        public HomeController(ILogger<HomeController> logger, MyDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Helper method to set cart count for the layout
        private void SetCartCount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                // User not logged in, use session
                var sessionCart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
                ViewData["CartItemCount"] = sessionCart.Sum(c => c.Quantity);
            }
            else
            {
                // User logged in, use database
                ViewData["CartItemCount"] = _context.CartItems.Where(c => c.UserId == userId).Sum(c => c.Quantity);
            }
        }

        public IActionResult Index()
        {
            SetCartCount(); // Set cart count for the header

            // Get all books from the database
            var books = _context.Books.ToList();

            // Group books by category and pass as a dictionary
            var groupedBooks = books
                .GroupBy(b => b.Category)
                .ToDictionary(g => g.Key, g => g.ToList());

            return View(groupedBooks); // Pass the grouped data to the view
        }

        public IActionResult Privacy()
        {
            SetCartCount(); // Set cart count for the header
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            SetCartCount(); // Set cart count for the header
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}