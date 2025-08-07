using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using TheBookHeaven.Helpers;
using TheBookHeaven.Models;

namespace TheBookHeaven.Controllers
{
    public class BookController : Controller
    {
        private readonly MyDbContext _context;

        public BookController(MyDbContext context)
        {
            _context = context;
        }

        // Book Categories
        // Action for Fiction Books
        public IActionResult Fiction()
        {
            var books = _context.Books.Where(b => b.Category == "Fiction").ToList();
            return View(books);  // Pass the filtered list to the view
        }

        // Action for Non-Fiction Books
        public IActionResult NonFiction()
        {
            var books = _context.Books.Where(b => b.Category == "Non-Fiction").ToList();
            return View(books);  // Pass the filtered list to the view
        }

        // Helper method to get cart count
        private int GetCartItemCount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                // User not logged in, use session
                var sessionCart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
                return sessionCart.Sum(c => c.Quantity);
            }
            else
            {
                // User logged in, use database
                return _context.CartItems.Where(c => c.UserId == userId).Sum(c => c.Quantity);
            }
        }

        // Add item to cart (for both logged-in and non-logged-in users)
        [HttpGet]
        public JsonResult AddToCart(int id)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == id);
            if (book == null)
                return Json(new { success = false, message = "Book not found." });

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId)) // User is not logged in
            {
                // Store cart in session for non-logged-in users
                var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
                var existingItem = cart.FirstOrDefault(ci => ci.Book?.Id == id);

                if (existingItem != null)
                {
                    existingItem.Quantity++;
                }
                else
                {
                    cart.Add(new CartItem { Book = book, BookId = id, Quantity = 1 });
                }

                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }
            else // User is logged in
            {
                // Check if item already exists in database cart
                var existingCartItem = _context.CartItems.FirstOrDefault(c => c.UserId == userId && c.BookId == id);

                if (existingCartItem != null)
                {
                    existingCartItem.Quantity++;
                    _context.CartItems.Update(existingCartItem);
                }
                else
                {
                    var newCartItem = new CartItem
                    {
                        BookId = id,
                        Quantity = 1,
                        UserId = userId
                    };
                    _context.CartItems.Add(newCartItem);
                }

                _context.SaveChanges();
            }

            var cartCount = GetCartItemCount();
            return Json(new { success = true, message = "1 item added to your cart.", cartCount = cartCount });
        }

        // View Cart (for both logged-in and non-logged-in users)
        public IActionResult Cart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<CartItem> cart;

            if (string.IsNullOrEmpty(userId)) // If not logged in, use session
            {
                cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            }
            else // If logged in, fetch cart from the database
            {
                cart = _context.CartItems
                    .Include(c => c.Book)
                    .Where(c => c.UserId == userId)
                    .ToList();
            }

            return View(cart);
        }

        // Remove item from cart
        [HttpPost]
        public IActionResult RemoveFromCart(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId)) // If not logged in, use session
            {
                var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
                var itemToRemove = cart.FirstOrDefault(ci => ci.Book?.Id == id);
                if (itemToRemove != null)
                {
                    cart.Remove(itemToRemove);
                    HttpContext.Session.SetObjectAsJson("Cart", cart);
                }
            }
            else // If logged in, remove from the database
            {
                var itemToRemove = _context.CartItems.FirstOrDefault(c => c.UserId == userId && c.BookId == id);
                if (itemToRemove != null)
                {
                    _context.CartItems.Remove(itemToRemove);
                    _context.SaveChanges();
                }
            }

            return RedirectToAction("Cart");
        }

        // Updated Increase quantity method
        [HttpPost]
        public JsonResult IncreaseQuantity(int id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                {
                    // Session-based cart for non-logged-in users
                    var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
                    var item = cart.FirstOrDefault(ci => ci.BookId == id || (ci.Book?.Id == id));

                    if (item != null)
                    {
                        item.Quantity++;
                        HttpContext.Session.SetObjectAsJson("Cart", cart);
                    }
                }
                else
                {
                    // Database cart for logged-in users
                    var cartItem = _context.CartItems.FirstOrDefault(c => c.UserId == userId && c.BookId == id);

                    if (cartItem != null)
                    {
                        cartItem.Quantity++;
                        _context.CartItems.Update(cartItem);
                        _context.SaveChanges();
                    }
                }

                var cartCount = GetCartItemCount();
                return Json(new { success = true, cartCount = cartCount });
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine($"Error in IncreaseQuantity: {ex.Message}");
                return Json(new { success = false, message = ex.Message });
            }
        }

        // Updated Decrease quantity method
        [HttpPost]
        public JsonResult DecreaseQuantity(int id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                {
                    // Session-based cart for non-logged-in users
                    var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
                    var item = cart.FirstOrDefault(ci => ci.BookId == id || (ci.Book?.Id == id));

                    if (item != null && item.Quantity > 1)
                    {
                        item.Quantity--;
                        HttpContext.Session.SetObjectAsJson("Cart", cart);
                    }
                }
                else
                {
                    // Database cart for logged-in users
                    var cartItem = _context.CartItems.FirstOrDefault(c => c.UserId == userId && c.BookId == id);

                    if (cartItem != null && cartItem.Quantity > 1)
                    {
                        cartItem.Quantity--;
                        _context.CartItems.Update(cartItem);
                        _context.SaveChanges();
                    }
                }

                var cartCount = GetCartItemCount();
                return Json(new { success = true, cartCount = cartCount });
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine($"Error in DecreaseQuantity: {ex.Message}");
                return Json(new { success = false, message = ex.Message });
            }
        }

        // Method to merge session cart with database cart when user logs in
        public void MergeSessionCartToUser(string userId)
        {
            var sessionCart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");
            if (sessionCart != null && sessionCart.Any())
            {
                foreach (var sessionItem in sessionCart)
                {
                    var existingDbItem = _context.CartItems
                        .FirstOrDefault(c => c.UserId == userId && c.BookId == sessionItem.BookId);

                    if (existingDbItem != null)
                    {
                        // Add session quantity to existing database item
                        existingDbItem.Quantity += sessionItem.Quantity;
                        _context.CartItems.Update(existingDbItem);
                    }
                    else
                    {
                        // Create new cart item in database
                        var newCartItem = new CartItem
                        {
                            UserId = userId,
                            BookId = sessionItem.BookId,
                            Quantity = sessionItem.Quantity
                        };
                        _context.CartItems.Add(newCartItem);
                    }
                }

                _context.SaveChanges();
                HttpContext.Session.Remove("Cart"); // Clear session cart after merging
            }
        }

        // Checkout action 
        public IActionResult Checkout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<CartItem> cart;

            if (string.IsNullOrEmpty(userId))
            {
                cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            }
            else
            {
                cart = _context.CartItems
                    .Include(c => c.Book)
                    .Where(c => c.UserId == userId)
                    .ToList();
            }

            return View(cart);
        }

        // Confirm order and save to database
        [HttpPost]
        public IActionResult ConfirmOrder()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userId = int.Parse(userIdClaim);

            // Get cart from database for logged-in users
            var cart = _context.CartItems
                .Include(c => c.Book)
                .Where(c => c.UserId == userIdClaim)
                .ToList();

            if (cart.Any())
            {
                decimal totalPrice = cart.Sum(item => item.Book.Price * item.Quantity);

                var order = new Order
                {
                    UserId = userIdClaim,
                    Status = "Processing",
                    OrderDate = DateTime.Now,
                    TotalPrice = totalPrice
                };

                try
                {
                    _context.Orders.Add(order);
                    _context.SaveChanges();

                    foreach (var cartItem in cart)
                    {
                        var orderItem = new OrderItem
                        {
                            OrderId = order.OrderId,
                            BookId = cartItem.BookId,
                            Quantity = cartItem.Quantity,
                            Price = cartItem.Book.Price
                        };

                        _context.OrderItems.Add(orderItem);
                    }

                    _context.SaveChanges();

                    // Clear user's cart from database
                    _context.CartItems.RemoveRange(cart);
                    _context.SaveChanges();

                    return RedirectToAction("MyOrder", "Order");
                }
                catch (Exception ex)
                {
                    TempData["Message"] = "Order created successfully! (Order details will be available soon)";
                    return RedirectToAction("MyOrder", "Order");
                }
            }

            return RedirectToAction("Cart");
        }

        // Index action list all books
        public IActionResult Index()
        {
            var allBooks = _context.Books.ToList();
            return View("Book", allBooks);
        }

        // Override the base controller method to set cart count for all actions
        public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context)
        {
            ViewData["CartItemCount"] = GetCartItemCount();
            base.OnActionExecuting(context);
        }

        // Search books
        [HttpGet]
        public IActionResult Search(string query, string category = "All", string sortBy = "Title")
        {
            var books = _context.Books.AsQueryable();

            // Filter by search query
            if (!string.IsNullOrEmpty(query))
            {
                books = books.Where(b => b.Title.Contains(query));
            }

            // Filter by category
            if (!string.IsNullOrEmpty(category) && category != "All")
            {
                books = books.Where(b => b.Category == category);
            }

            // Sort books
            books = sortBy switch
            {
                "Title" => books.OrderBy(b => b.Title),
                "PriceLowToHigh" => books.OrderBy(b => b.Price),
                "PriceHighToLow" => books.OrderByDescending(b => b.Price),
                "Newest" => books.OrderByDescending(b => b.Id),
                _ => books.OrderBy(b => b.Title)
            };

            // Get available categories for the filter dropdown
            var categories = _context.Books.Select(b => b.Category).Distinct().ToList();

            ViewBag.Query = query;
            ViewBag.SelectedCategory = category;
            ViewBag.SelectedSort = sortBy;
            ViewBag.Categories = categories;
            ViewBag.ResultCount = books.Count();

            return View("Search", books.ToList());
        }

        // Browse all books with filtering
        public IActionResult Browse(string category = "All", string sortBy = "Title")
        {
            return Search("", category, sortBy);
        }

    }
}