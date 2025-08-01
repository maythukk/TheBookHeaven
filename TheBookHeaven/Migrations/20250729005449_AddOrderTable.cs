using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheBookHeaven.Migrations
{
    public partial class AddOrderTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.OrderId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the Order table if rolling back the migration
            migrationBuilder.DropTable(name: "Order");

            // Insert default books back if needed
            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Category", "ImageUrl", "Price", "Title" },
                values: new object[,]
                {
                    { 1, "Best Sellers", "https://cdn.britannica.com/04/126004-050-EC4DF54F/Dustcover-Louisa-May-Alcott-Little-Women-novel.jpg", 19.99m, "Little Women" },
                    { 2, "Fiction", "https://assets.lulu.com/cover_thumbs/8/d/8dzdnj-front-shortedge-384.jpg", 14.99m, "The Great Gatsby" },
                    { 3, "Non-Fiction", "https://images-na.ssl-images-amazon.com/images/S/compressed.photo.goodreads.com/books/1703329310i/23692271.jpg", 21.50m, "Sapiens" },
                    { 4, "Best Sellers", "https://m.media-amazon.com/images/I/71ls-I6A5KL.jpg", 17.99m, "The Midnight Library" },
                    { 5, "Best Sellers", "https://m.media-amazon.com/images/I/81e+mSqZvnL._UF1000,1000_QL80_.jpg", 16.50m, "Where the Crawdads Sing" },
                    { 6, "Best Sellers", "https://m.media-amazon.com/images/I/612KmKeEYEL._UF1000,1000_QL80_.jpg", 17.25m, "Crime and Punishment" },
                    { 7, "Fiction", "https://bci.kinokuniya.com/jsp/images/book-img/97801/97801410/9780141036144.JPG", 15.00m, "1984" },
                    { 8, "Fiction", "https://m.media-amazon.com/images/I/81aY1lxk+9L.jpg", 13.50m, "To Kill a Mockingbird" },
                    { 9, "Fiction", "https://cdn.kobo.com/book-images/fdeeeaac-142d-44fc-8484-698fcf5e6378/1200/1200/False/the-overcoat-and-other-short-stories.jpg", 11.99m, "The Overcoat and Other Short Stories" },
                    { 10, "Non-Fiction", "https://m.media-amazon.com/images/I/71N2HZwRo3L.jpg", 18.99m, "Educated" },
                    { 11, "Non-Fiction", "https://m.media-amazon.com/images/I/81ANaVZk5LL.jpg", 20.99m, "Atomic Habits" },
                    { 12, "Non-Fiction", "https://m.media-amazon.com/images/I/71wm29Etl4L.jpg", 16.95m, "The Power of Habit" }
                });
        }
    }
}
