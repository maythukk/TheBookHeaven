using Microsoft.AspNetCore.Mvc;
using TheBookHeaven.Models;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TheBookHeaven.Helpers;

namespace TheBookHeaven.Controllers
{
    public class BookController : Controller
    {
        private readonly MyDbContext _context;

        public BookController(MyDbContext context)
        {
            _context = context;
        }

        public IActionResult BestSellers()
        {
            return View();
        }

        public IActionResult Fiction()
        {
            return View();
        }

        public IActionResult NonFiction()
        {
            return View();
        }

        [HttpGet]
        public JsonResult AddToCart(int id)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == id);
            if (book == null)
                return Json(new { success = false, message = "Book not found." });

            List<CartItem> cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            var existingItem = cart.FirstOrDefault(ci => ci.Book.Id == id);

            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                cart.Add(new CartItem { Book = book, Quantity = 1 });
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return Json(new { success = true, message = "1 item added to your cart." });
        }

        public IActionResult Cart()
        {
            List<CartItem> cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            return View(cart);
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int id)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            var itemToRemove = cart.FirstOrDefault(ci => ci.Book.Id == id);
            if (itemToRemove != null)
            {
                cart.Remove(itemToRemove);
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }

            return RedirectToAction("Cart");
        }
        public IActionResult Index()
        {
            var allBooks = _context.Books.ToList();
            return View("Book", allBooks); 
        }

    }
}
