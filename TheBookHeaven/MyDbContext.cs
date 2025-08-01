using Microsoft.EntityFrameworkCore;
using TheBookHeaven.Models;

namespace TheBookHeaven
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        // DbSet for Books
        public DbSet<Book> Books { get; set; }

        // DbSet for CartItems
        public DbSet<CartItem> CartItems { get; set; }

        // DbSet for Users
        public DbSet<User> Users { get; set; }

        // DbSet for Orders
        public DbSet<Order> Orders { get; set; }

        // DbSet for OrderItems
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Fix table naming consistency
            modelBuilder.Entity<Order>().ToTable("Orders"); 

            modelBuilder.Entity<OrderItem>().ToTable("OrderItems");

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalPrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.Price)
                .HasColumnType("decimal(18,2)");

            // Simplified relationship configuration
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Book)
                .WithMany()
                .HasForeignKey(oi => oi.BookId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}