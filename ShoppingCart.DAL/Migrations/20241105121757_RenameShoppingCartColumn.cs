using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingCart.DAL.Migrations
{
    public partial class RenameShoppingCartColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CartCount",
                table: "ShoppingCarts",
                newName: "Count");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Count",
                table: "ShoppingCarts",
                newName: "CartCount");
        }
    }
}
