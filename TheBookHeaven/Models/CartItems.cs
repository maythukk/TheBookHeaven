namespace TheBookHeaven.Models
{
    public class CartItem
    {
        public int Id { get; set; }  // Primary key
        public string UserId { get; set; }  // Foreign key for the user
        public int BookId { get; set; }  // Foreign key for the book
        public Book Book { get; set; }  // Navigation property to Book
        public int Quantity { get; set; }  // Quantity of the book in the cart
    }
}