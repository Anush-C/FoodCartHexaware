using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodCart_Hexaware.Migrations
{
    /// <inheritdoc />
    public partial class RestaurantCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MenuCategoryRestaurant",
                columns: table => new
                {
                    MenuCategoriesCategoryID = table.Column<int>(type: "int", nullable: false),
                    RestaurantsRestaurantID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuCategoryRestaurant", x => new { x.MenuCategoriesCategoryID, x.RestaurantsRestaurantID });
                    table.ForeignKey(
                        name: "FK_MenuCategoryRestaurant_MenuCategories_MenuCategoriesCategoryID",
                        column: x => x.MenuCategoriesCategoryID,
                        principalTable: "MenuCategories",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuCategoryRestaurant_Restaurants_RestaurantsRestaurantID",
                        column: x => x.RestaurantsRestaurantID,
                        principalTable: "Restaurants",
                        principalColumn: "RestaurantID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MenuCategoryRestaurant_RestaurantsRestaurantID",
                table: "MenuCategoryRestaurant",
                column: "RestaurantsRestaurantID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MenuCategoryRestaurant");
        }
    }
}
