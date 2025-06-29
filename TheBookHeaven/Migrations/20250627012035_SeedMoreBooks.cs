using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TheBookHeaven.Migrations
{
    /// <inheritdoc />
    public partial class SeedMoreBooks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Category", "ImageUrl", "Price", "Title" },
                values: new object[,]
                {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 12);
        }
    }
}
