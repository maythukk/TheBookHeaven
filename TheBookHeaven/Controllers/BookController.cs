using Microsoft.AspNetCore.Mvc;
namespace TheBookHeaven.Controllers
{
    public class BookController : Controller
    {
        public IActionResult BestSellers()
        {
            return View(); // Returns the view for the Best Sellers page
        }
        public IActionResult Fiction()
        {
            return View(); // Returns the view for the Fiction page
        }
        public IActionResult NonFiction()
        {
            return View(); // Returns the view for the Non-Fiction page
        }
    }
}