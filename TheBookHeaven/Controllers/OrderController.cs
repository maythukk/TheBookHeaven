using Microsoft.AspNetCore.Mvc;
namespace TheBookHeaven.Controllers
{
    public class OrderController : Controller
    {
        // Renders the MyOrder.cshtml view
        public IActionResult MyOrder()
        {
            return View(); 
        }
    }
}