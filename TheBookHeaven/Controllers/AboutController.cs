using Microsoft.AspNetCore.Mvc;
namespace TheBookHeaven.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Aboutus()
        {
            return View(); // Returns the view for the About Us page
        }
    }
}

