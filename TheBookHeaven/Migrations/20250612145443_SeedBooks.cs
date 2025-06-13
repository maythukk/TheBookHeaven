using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TheBookHeaven.Migrations
{
    /// <inheritdoc />
    public partial class SeedBooks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Category", "ImageUrl", "Price", "Title" },
                values: new object[,]
                {
                    { 1, "Best Sellers", "https://m.media-amazon.com/images/I/71jDBMuqt9L._AC_UF1000,1000_QL80_.jpg", 19.99m, "Little Women" },
                    { 2, "Fiction", "https://assets.lulu.com/cover_thumbs/8/d/8dzdnj-front-shortedge-384.jpg", 14.99m, "The Great Gatsby" },
                    { 3, "Non-Fiction", "https://images-na.ssl-images-amazon.com/images/S/compressed.photo.goodreads.com/books/1703329310i/23692271.jpg", 21.50m, "Sapiens" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
