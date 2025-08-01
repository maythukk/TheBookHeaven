using System;
using System.Collections.Generic;

namespace TheBookHeaven.Models
{
    public class Order
    {
        public int OrderId { get; set; }  // Primary key for Order
        public string UserId { get; set; }  // Foreign key for the user who made the order
        public string Status { get; set; }  // Order status (e.g., "Processing", "Shipped", "Delivered")
        public DateTime OrderDate { get; set; }  // Date when the order was placed
        public decimal TotalPrice { get; set; }  // Total price of the order

        // Navigation property - Collection of OrderItems
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public bool CancellationRequested { get; set; }  // Flag to track cancellation request

    }
}