using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingCart.DAL.Migrations
{
    public partial class ChangeNameOfApplicationUserIdInUserShoppingCartTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCarts_AspNetUsers_ApplicationUserId",
                table: "ShoppingCarts");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "ShoppingCarts",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingCarts_ApplicationUserId",
                table: "ShoppingCarts",
                newName: "IX_ShoppingCarts_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCarts_AspNetUsers_UserId",
                table: "ShoppingCarts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCarts_AspNetUsers_UserId",
                table: "ShoppingCarts");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ShoppingCarts",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingCarts_UserId",
                table: "ShoppingCarts",
                newName: "IX_ShoppingCarts_ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCarts_AspNetUsers_ApplicationUserId",
                table: "ShoppingCarts",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
