using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheBookHeaven.Models;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;

namespace TheBookHeaven.Controllers
{
    public class OrderController : Controller
    {
        private readonly MyDbContext _context;

        public OrderController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Order/MyOrder
        public async Task<IActionResult> MyOrder(string viewType)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Check if the user is logged in
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            List<Order> orders;

            if (viewType == "history")
            {
                // Fetch history orders (Delivered and Cancelled)
                orders = await _context.Orders
                    .Where(o => o.UserId == userId && (o.Status == "Delivered" || o.Status == "Cancelled"))
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Book)
                    .OrderByDescending(o => o.OrderDate)
                    .ToListAsync();
            }
            else
            {
                // Fetch current orders (Processing and Shipped)
                orders = await _context.Orders
                    .Where(o => o.UserId == userId && (o.Status == "Processing" || o.Status == "Shipped"))
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Book)
                    .OrderByDescending(o => o.OrderDate)
                    .ToListAsync();
            }

            // Return the orders view
            return View(orders);
        }

        // POST: Order cancellation method
        [HttpPost]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderId == id && o.UserId == userId);

            if (order == null)
            {
                return NotFound();
            }

            // Check if the order is in "Processing" status and can be canceled
            if (order.Status == "Processing")
            {
                // Mark the order as a cancellation request (set CancellationRequested to true)
                order.CancellationRequested = true;

                // Save the changes
                _context.Update(order);
                await _context.SaveChangesAsync();

                // Store a success message in TempData
                TempData["SuccessMessage"] = "Your cancellation request has been sent. Please wait for admin approval.";

                // Redirect back to My Orders page
                return RedirectToAction(nameof(MyOrder));
            }

            // If the order is not "Processing", prevent cancellation
            TempData["ErrorMessage"] = "Order cannot be cancelled because it is already shipped or delivered.";
            return RedirectToAction(nameof(MyOrder));
        }
    }
}
