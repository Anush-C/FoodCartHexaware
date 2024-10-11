using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FoodCart_Hexaware.Migrations
{
    /// <inheritdoc />
    public partial class Address : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "RestaurantID",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "RestaurantID",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "RestaurantID",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "RestaurantID",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "RestaurantID",
                keyValue: 15);

            migrationBuilder.AlterColumn<string>(
                name: "DeliveryAddress",
                table: "Carts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DeliveryAddress",
                table: "Carts",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "RestaurantID", "ClosingHours", "OpeningHours", "RestaurantAddress", "RestaurantDescription", "RestaurantEmail", "RestaurantName", "RestaurantPhone" },
                values: new object[,]
                {
                    { 11, new TimeSpan(0, 22, 0, 0, 0), new TimeSpan(0, 11, 0, 0, 0), "808 Sushi St", "Fresh sushi and sashimi with a modern twist.", "info@sushispot.com", "Sushi Spot", "1357924680" },
                    { 12, new TimeSpan(0, 21, 0, 0, 0), new TimeSpan(0, 9, 0, 0, 0), "909 Greenway Rd", "Delicious plant-based meals for everyone.", "contact@veganparadise.com", "Vegan Paradise", "2468135790" },
                    { 13, new TimeSpan(0, 23, 0, 0, 0), new TimeSpan(0, 10, 0, 0, 0), "121 Taco St", "Authentic tacos and Mexican street food.", "info@tacotown.com", "Taco Town", "3579246801" },
                    { 14, new TimeSpan(0, 23, 0, 0, 0), new TimeSpan(0, 17, 0, 0, 0), "131 Steak St", "Premium cuts of meat grilled to perfection.", "contact@steakhousesupreme.com", "Steakhouse Supreme", "4681357920" },
                    { 15, new TimeSpan(0, 22, 0, 0, 0), new TimeSpan(0, 11, 0, 0, 0), "141 Ocean Blvd", "Fresh seafood delivered daily.", "info@thefishmarket.com", "The Fish Market", "5792468130" }
                });
        }
    }
}
