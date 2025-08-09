using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TheBookHeaven;
using TheBookHeaven.Models;

public class AdminController : Controller
{
    private readonly MyDbContext _context;

    public AdminController(MyDbContext context)
    {
        _context = context;
    }

    // GET: Admin/Dashboard
    public IActionResult Dashboard()
    {
        var role = HttpContext.Session.GetString("Role");
        if (role != "Admin")
        {
            return RedirectToAction("Login", "Account");
        }

        ViewBag.Username = HttpContext.Session.GetString("Username");
        return View();
    }

    // GET: Admin/ViewBooks
    public IActionResult ViewBooks()
    {
        var books = _context.Books.ToList();
        return View(books);
    }

    // GET: Admin/Edit/{id}
    public IActionResult Edit(int id)
    {
        var book = _context.Books.FirstOrDefault(b => b.Id == id);
        if (book == null)
        {
            return NotFound();
        }

        return View(book);
    }

    // POST: Admin/Edit/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Book updatedBook)
    {
        if (id != updatedBook.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var book = _context.Books.Find(updatedBook.Id);
                if (book == null)
                {
                    return NotFound();
                }

                // Update book properties, including stock quantity
                book.Title = updatedBook.Title;
                book.Category = updatedBook.Category;
                book.ImageUrl = updatedBook.ImageUrl;
                book.Price = updatedBook.Price;
                book.StockQuantity = updatedBook.StockQuantity;  // Update stock quantity

                _context.SaveChanges();

                return RedirectToAction("ViewBooks");
            }
            catch (Exception ex)
            {
                // Log the exception 
                ModelState.AddModelError("", "An error occurred while saving the book. Please try again.");
                return View(updatedBook);
            }
        }

        // If validation failed, return to view with validation errors
        return View(updatedBook);
    }

    // POST: Admin/DeleteBook/{id}
    [HttpGet]
    public IActionResult DeleteBook(int id)
    {
        var book = _context.Books.FirstOrDefault(b => b.Id == id);
        if (book == null)
        {
            return NotFound();
        }

        _context.Books.Remove(book);
        _context.SaveChanges();

        return RedirectToAction("ViewBooks");
    }

    // GET: Admin/AddBook
    public IActionResult AddBook()
    {
        return View();
    }

    // POST: Admin/AddBook
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddBook(Book book)
    {
        if (ModelState.IsValid)
        {
            // Add stock quantity to the book
            _context.Books.Add(book);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Book added successfully!";
            return RedirectToAction("ViewBooks");
        }

        return View(book);
    }

    // GET: Admin/ViewUsers
    public IActionResult ViewUsers()
    {
        var role = HttpContext.Session.GetString("Role");
        if (role != "Admin")
        {
            return RedirectToAction("Login", "Account");
        }

        // Fetch all users except admins
        var users = _context.Users.Where(u => u.Role != "Admin").ToList();

        return View(users);
    }

    // POST: Admin/DeleteUser/{id}
    [HttpPost]
    public IActionResult DeleteUser(int id)
    {
        var role = HttpContext.Session.GetString("Role");
        if (role != "Admin")
        {
            return RedirectToAction("Login", "Account");
        }

        var user = _context.Users.Find(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        return RedirectToAction("ViewUsers");
    }

    // GET: Admin/ViewOrders
    public async Task<IActionResult> ViewOrders(string status)
    {
        var orders = _context.Orders
                             .Include(o => o.OrderItems) // Include order items
                                 .ThenInclude(oi => oi.Book) // Include book details for each order item
                             .OrderBy(o => o.Status == "Cancelled" ? 1 : 0) // Place cancelled orders last
                             .ThenBy(o => o.Status)  // Sort by status 
                             .ThenByDescending(o => o.OrderDate) // Then sort by order date
                             .AsQueryable();

        if (!string.IsNullOrEmpty(status))
        {
            orders = orders.Where(o => o.Status == status); // Filter by specific status if selected
        }

        return View(await orders.ToListAsync());
    }

    // POST: Admin/UpdateOrderStatus/{id}
    [HttpPost]
    public async Task<IActionResult> UpdateOrderStatus(int id, string status)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
        {
            return NotFound();
        }

        // Update order status
        order.Status = status;
        _context.Update(order);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(ViewOrders));
    }

    // POST: Admin approve cancellation
    [HttpPost]
    public async Task<IActionResult> ApproveCancellation(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null || !order.CancellationRequested)
        {
            return NotFound(); // Or show an error message
        }

        // Approve the cancellation request and set the order status to "Cancelled"
        order.Status = "Cancelled";
        order.CancellationRequested = false; // Reset cancellation request flag
        _context.Update(order);
        await _context.SaveChangesAsync();

        // Store rejection message in TempData to inform the customer
        TempData["SuccessMessage"] = "Your cancellation request was approved. Your order is cancelled.";

        return RedirectToAction(nameof(ViewOrders)); // Redirect to admin's order management page
    }

    // POST: Admin reject cancellation
    [HttpPost]
    public async Task<IActionResult> RejectCancellation(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
        {
            return NotFound();
        }

        // Set the CancellationRequested flag to false if the cancellation is rejected
        order.CancellationRequested = false;
        _context.Update(order);
        await _context.SaveChangesAsync();

        // Store rejection message in TempData to inform the customer
        TempData["ErrorMessage"] = "Your cancellation request was rejected. Your order will proceed as normal.";

        return RedirectToAction(nameof(ViewOrders));
    }
}