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
            // Seed data removed
        }
    }
}
