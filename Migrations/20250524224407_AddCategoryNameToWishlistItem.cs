using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tradie.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryNameToWishlistItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                table: "WishlistItems",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryName",
                table: "WishlistItems");
        }
    }
}
