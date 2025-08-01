using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBookHeaven.Models
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }  // Primary key for OrderItem

        // Remove [ForeignKey] attributes from the foreign key properties
        public int OrderId { get; set; }  // Foreign key for the Order
        public int BookId { get; set; }  // Foreign key for the Book

        public int Quantity { get; set; }  // Quantity of the book in the order
        public decimal Price { get; set; }  // Price of the book at the time of order

        // Apply [ForeignKey] to navigation properties instead
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        [ForeignKey("BookId")]
        public virtual Book Book { get; set; }
    }
}