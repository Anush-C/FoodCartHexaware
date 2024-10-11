using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodCart_Hexaware.Migrations
{
    /// <inheritdoc />
    public partial class cartrest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RestaurantID",
                table: "Carts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Carts_RestaurantID",
                table: "Carts",
                column: "RestaurantID");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Restaurants_RestaurantID",
                table: "Carts",
                column: "RestaurantID",
                principalTable: "Restaurants",
                principalColumn: "RestaurantID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Restaurants_RestaurantID",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_Carts_RestaurantID",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "RestaurantID",
                table: "Carts");
        }
    }
}
