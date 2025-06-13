using Microsoft.EntityFrameworkCore;
using TheBookHeaven.Models;

namespace TheBookHeaven
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().HasData(
                new Book
                {
                    Id = 1,
                    Title = "Little Women",
                    Price = 19.99m,
                    Category = "Best Sellers",
                    ImageUrl = "https://cdn.britannica.com/04/126004-050-EC4DF54F/Dustcover-Louisa-May-Alcott-Little-Women-novel.jpg"
                },
                new Book
                {
                    Id = 2,
                    Title = "The Great Gatsby",
                    Price = 14.99m,
                    Category = "Fiction",
                    ImageUrl = "https://assets.lulu.com/cover_thumbs/8/d/8dzdnj-front-shortedge-384.jpg"
                },
                new Book
                {
                    Id = 3,
                    Title = "Sapiens",
                    Price = 21.50m,
                    Category = "Non-Fiction",
                    ImageUrl = "https://images-na.ssl-images-amazon.com/images/S/compressed.photo.goodreads.com/books/1703329310i/23692271.jpg"
                }
            );
        }

    }
}
