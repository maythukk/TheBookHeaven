using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBookHeaven.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }

        [Precision(10, 2)]
        public decimal Price { get; set; }

        public int StockQuantity { get; set; }  

        // New computed properties for convenience
        [NotMapped]
        public bool IsInStock => StockQuantity > 0;

        [NotMapped]
        public bool IsLowStock => StockQuantity > 0 && StockQuantity <= 5;
    }
}