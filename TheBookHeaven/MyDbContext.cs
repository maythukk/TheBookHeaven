using Microsoft.EntityFrameworkCore;
using TheBookHeaven.Models;

namespace TheBookHeaven
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }
        
        // Book table
        public DbSet<Book> Books { get; set; }

        // User table
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().HasData(
                // Best Sellers 
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
                    Id = 4,
                    Title = "The Midnight Library",
                    Price = 17.99m,
                    Category = "Best Sellers",
                    ImageUrl = "https://m.media-amazon.com/images/I/71ls-I6A5KL.jpg"
                },
                new Book
                {
                    Id = 5,
                    Title = "Where the Crawdads Sing",
                    Price = 16.50m,
                    Category = "Best Sellers",
                    ImageUrl = "https://m.media-amazon.com/images/I/81e+mSqZvnL._UF1000,1000_QL80_.jpg"
                },
                new Book
                {
                    Id = 6,
                    Title = "Crime and Punishment",
                    Price = 17.25m,
                    Category = "Best Sellers",
                    ImageUrl = "https://m.media-amazon.com/images/I/612KmKeEYEL._UF1000,1000_QL80_.jpg"
                },

                // Fiction 
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
                    Id = 7,
                    Title = "1984",
                    Price = 15.00m,
                    Category = "Fiction",
                    ImageUrl = "https://bci.kinokuniya.com/jsp/images/book-img/97801/97801410/9780141036144.JPG"
                },
                new Book
                {
                    Id = 8,
                    Title = "To Kill a Mockingbird",
                    Price = 13.50m,
                    Category = "Fiction",
                    ImageUrl = "https://m.media-amazon.com/images/I/81aY1lxk+9L.jpg"
                },
                new Book
                {
                    Id = 9,
                    Title = "The Overcoat and Other Short Stories",
                    Price = 11.99m,
                    Category = "Fiction",
                    ImageUrl = "https://cdn.kobo.com/book-images/fdeeeaac-142d-44fc-8484-698fcf5e6378/1200/1200/False/the-overcoat-and-other-short-stories.jpg"
                },

                // Non-Fiction 
                new Book
                {
                    Id = 3,
                    Title = "Sapiens",
                    Price = 21.50m,
                    Category = "Non-Fiction",
                    ImageUrl = "https://images-na.ssl-images-amazon.com/images/S/compressed.photo.goodreads.com/books/1703329310i/23692271.jpg"
                },
                new Book
                {
                    Id = 10,
                    Title = "Educated",
                    Price = 18.99m,
                    Category = "Non-Fiction",
                    ImageUrl = "https://m.media-amazon.com/images/I/71N2HZwRo3L.jpg"
                },
                new Book
                {
                    Id = 11,
                    Title = "Atomic Habits",
                    Price = 20.99m,
                    Category = "Non-Fiction",
                    ImageUrl = "https://m.media-amazon.com/images/I/81ANaVZk5LL.jpg"
                },
                new Book
                {
                    Id = 12,
                    Title = "The Power of Habit",
                    Price = 16.95m,
                    Category = "Non-Fiction",
                    ImageUrl = "https://m.media-amazon.com/images/I/71wm29Etl4L.jpg"
                }
            );
        }


    }
}
