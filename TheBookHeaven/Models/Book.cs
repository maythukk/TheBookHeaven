using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBookHeaven.Models
{
    public class Book
    {
        public int Id { get; set; } // primary key
        public string Title { get; set; }
        public string Category { get; set; }

        [Precision(10, 2)] // Max 10 digits, 2 digits after the decimal point
        public decimal Price { get; set; }

        public string ImageUrl { get; set; }
    }
}
