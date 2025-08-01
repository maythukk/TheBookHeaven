using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheBookHeaven.Migrations
{
    /// <inheritdoc />
    public partial class AddCancellationRequestedFlag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CancellationRequested",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancellationRequested",
                table: "Orders");
        }
    }
}
