using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tradie.Migrations
{
	/// <inheritdoc />
	public partial class AddWishlistIdToWishlistItem : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_CartItem_Products_ProductId",
				table: "CartItem");

			migrationBuilder.DropForeignKey(
				name: "FK_CartItem_ShoppingCarts_ShoppingCartId",
				table: "CartItem");

			migrationBuilder.DropPrimaryKey(
				name: "PK_CartItem",
				table: "CartItem");

			migrationBuilder.DropColumn(
				name: "DeliveryFee",
				table: "ShoppingCarts");

			migrationBuilder.DropColumn(
				name: "Subtotal",
				table: "ShoppingCarts");

			migrationBuilder.DropColumn(
				name: "Total",
				table: "ShoppingCarts");

			migrationBuilder.DropColumn(
				name: "Address",
				table: "AspNetUsers");

			migrationBuilder.DropColumn(
				name: "Age",
				table: "AspNetUsers");

			migrationBuilder.DropColumn(
				name: "Phone",
				table: "AspNetUsers");

			migrationBuilder.DropColumn(
				name: "password",
				table: "AspNetUsers");

			migrationBuilder.RenameTable(
				name: "CartItem",
				newName: "CartItems");

			migrationBuilder.RenameIndex(
				name: "IX_CartItem_ShoppingCartId",
				table: "CartItems",
				newName: "IX_CartItems_ShoppingCartId");

			migrationBuilder.RenameIndex(
				name: "IX_CartItem_ProductId",
				table: "CartItems",
				newName: "IX_CartItems_ProductId");

			migrationBuilder.AddColumn<string>(
				name: "UserId",
				table: "ShoppingCarts",
				type: "nvarchar(max)",
				nullable: false,
				defaultValue: "");

			migrationBuilder.AddColumn<int>(
				name: "UserId1",
				table: "ShoppingCarts",
				type: "int",
				nullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "Email",
				table: "AspNetUsers",
				type: "nvarchar(256)",
				maxLength: 256,
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(256)",
				oldMaxLength: 256);

			migrationBuilder.AddColumn<int>(
				name: "WishlistId",
				table: "AspNetUsers",
				type: "int",
				nullable: true);

			migrationBuilder.AddPrimaryKey(
				name: "PK_CartItems",
				table: "CartItems",
				column: "Id");

			migrationBuilder.CreateTable(
				name: "Wishlists",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
					UserId1 = table.Column<int>(type: "int", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Wishlists", x => x.Id);
					table.ForeignKey(
						name: "FK_Wishlists_AspNetUsers_UserId1",
						column: x => x.UserId1,
						principalTable: "AspNetUsers",
						principalColumn: "Id");
				});

			migrationBuilder.CreateTable(
				name: "WishlistItems",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					ProductId = table.Column<int>(type: "int", nullable: false),
					ProductName = table.Column<string>(type: "nvarchar(max)", nullable: true),
					ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Quantity = table.Column<int>(type: "int", nullable: false),
					PriceAtAddition = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
					Size = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
					WishlistId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_WishlistItems", x => x.Id);
					table.ForeignKey(
						name: "FK_WishlistItems_Products_ProductId",
						column: x => x.ProductId,
						principalTable: "Products",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_WishlistItems_Wishlists_WishlistId",
						column: x => x.WishlistId,
						principalTable: "Wishlists",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_ShoppingCarts_UserId1",
				table: "ShoppingCarts",
				column: "UserId1");

			migrationBuilder.CreateIndex(
				name: "IX_AspNetUsers_WishlistId",
				table: "AspNetUsers",
				column: "WishlistId");

			migrationBuilder.CreateIndex(
				name: "IX_WishlistItems_ProductId",
				table: "WishlistItems",
				column: "ProductId");

			migrationBuilder.CreateIndex(
				name: "IX_WishlistItems_WishlistId",
				table: "WishlistItems",
				column: "WishlistId");

			migrationBuilder.CreateIndex(
				name: "IX_Wishlists_UserId1",
				table: "Wishlists",
				column: "UserId1");

			migrationBuilder.AddForeignKey(
				name: "FK_AspNetUsers_Wishlists_WishlistId",
				table: "AspNetUsers",
				column: "WishlistId",
				principalTable: "Wishlists",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_CartItems_Products_ProductId",
				table: "CartItems",
				column: "ProductId",
				principalTable: "Products",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_CartItems_ShoppingCarts_ShoppingCartId",
				table: "CartItems",
				column: "ShoppingCartId",
				principalTable: "ShoppingCarts",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_ShoppingCarts_AspNetUsers_UserId1",
				table: "ShoppingCarts",
				column: "UserId1",
				principalTable: "AspNetUsers",
				principalColumn: "Id");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_AspNetUsers_Wishlists_WishlistId",
				table: "AspNetUsers");

			migrationBuilder.DropForeignKey(
				name: "FK_CartItems_Products_ProductId",
				table: "CartItems");

			migrationBuilder.DropForeignKey(
				name: "FK_CartItems_ShoppingCarts_ShoppingCartId",
				table: "CartItems");

			migrationBuilder.DropForeignKey(
				name: "FK_ShoppingCarts_AspNetUsers_UserId1",
				table: "ShoppingCarts");

			migrationBuilder.DropTable(
				name: "WishlistItems");

			migrationBuilder.DropTable(
				name: "Wishlists");

			migrationBuilder.DropIndex(
				name: "IX_ShoppingCarts_UserId1",
				table: "ShoppingCarts");

			migrationBuilder.DropIndex(
				name: "IX_AspNetUsers_WishlistId",
				table: "AspNetUsers");

			migrationBuilder.DropPrimaryKey(
				name: "PK_CartItems",
				table: "CartItems");

			migrationBuilder.DropColumn(
				name: "UserId",
				table: "ShoppingCarts");

			migrationBuilder.DropColumn(
				name: "UserId1",
				table: "ShoppingCarts");

			migrationBuilder.DropColumn(
				name: "WishlistId",
				table: "AspNetUsers");

			migrationBuilder.RenameTable(
				name: "CartItems",
				newName: "CartItem");

			migrationBuilder.RenameIndex(
				name: "IX_CartItems_ShoppingCartId",
				table: "CartItem",
				newName: "IX_CartItem_ShoppingCartId");

			migrationBuilder.RenameIndex(
				name: "IX_CartItems_ProductId",
				table: "CartItem",
				newName: "IX_CartItem_ProductId");

			migrationBuilder.AddColumn<decimal>(
				name: "DeliveryFee",
				table: "ShoppingCarts",
				type: "decimal(18,2)",
				nullable: false,
				defaultValue: 0m);

			migrationBuilder.AddColumn<decimal>(
				name: "Subtotal",
				table: "ShoppingCarts",
				type: "decimal(18,2)",
				nullable: false,
				defaultValue: 0m);

			migrationBuilder.AddColumn<decimal>(
				name: "Total",
				table: "ShoppingCarts",
				type: "decimal(18,2)",
				nullable: false,
				defaultValue: 0m);

			migrationBuilder.AlterColumn<string>(
				name: "Email",
				table: "AspNetUsers",
				type: "nvarchar(256)",
				maxLength: 256,
				nullable: false,
				defaultValue: "",
				oldClrType: typeof(string),
				oldType: "nvarchar(256)",
				oldMaxLength: 256,
				oldNullable: true);

			migrationBuilder.AddColumn<string>(
				name: "Address",
				table: "AspNetUsers",
				type: "nvarchar(max)",
				nullable: false,
				defaultValue: "");

			migrationBuilder.AddColumn<int>(
				name: "Age",
				table: "AspNetUsers",
				type: "int",
				nullable: false,
				defaultValue: 0);

			migrationBuilder.AddColumn<int>(
				name: "Phone",
				table: "AspNetUsers",
				type: "int",
				nullable: false,
				defaultValue: 0);

			migrationBuilder.AddColumn<string>(
				name: "password",
				table: "AspNetUsers",
				type: "nvarchar(max)",
				nullable: false,
				defaultValue: "");

			migrationBuilder.AddPrimaryKey(
				name: "PK_CartItem",
				table: "CartItem",
				column: "Id");

			migrationBuilder.AddForeignKey(
				name: "FK_CartItem_Products_ProductId",
				table: "CartItem",
				column: "ProductId",
				principalTable: "Products",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_CartItem_ShoppingCarts_ShoppingCartId",
				table: "CartItem",
				column: "ShoppingCartId",
				principalTable: "ShoppingCarts",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);
		}
	}
}
