namespace TheBookHeaven.Models
{
    public class BookDetails
    {
        public Book Book { get; set; }
        public List<Book> SimilarBooks { get; set; }
    }
}