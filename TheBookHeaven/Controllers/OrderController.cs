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
        public async Task<IActionResult> MyOrder()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            // Fetch orders and include order items and related book information
            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)        // Include order items
                    .ThenInclude(oi => oi.Book)    // Include book details for each order item
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View(orders);
        }

        // GET: Order tracking method
        public async Task<IActionResult> TrackOrder(int id)
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

            return View(order);
        }

        // POST: Order cancellation method (customer request)
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
