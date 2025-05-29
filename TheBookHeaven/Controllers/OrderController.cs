<<<<<<< HEAD
﻿namespace TheBookHeaven.Controllers
{
    public class OrderController
    {
=======
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
>>>>>>> 07d9c12d846f5ca764ab8cd01f5942a4a8ae41bb
    }
}
