using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodCart_Hexaware.Migrations
{
    /// <inheritdoc />
    public partial class Ratings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Rating",
                table: "Menus",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "ItemID",
                keyValue: 1,
                column: "Rating",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "ItemID",
                keyValue: 2,
                column: "Rating",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "ItemID",
                keyValue: 3,
                column: "Rating",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "ItemID",
                keyValue: 4,
                column: "Rating",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "ItemID",
                keyValue: 5,
                column: "Rating",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "ItemID",
                keyValue: 6,
                column: "Rating",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "ItemID",
                keyValue: 7,
                column: "Rating",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "ItemID",
                keyValue: 8,
                column: "Rating",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "ItemID",
                keyValue: 9,
                column: "Rating",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "ItemID",
                keyValue: 10,
                column: "Rating",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "ItemID",
                keyValue: 11,
                column: "Rating",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "ItemID",
                keyValue: 12,
                column: "Rating",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "ItemID",
                keyValue: 13,
                column: "Rating",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "ItemID",
                keyValue: 14,
                column: "Rating",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "ItemID",
                keyValue: 15,
                column: "Rating",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "ItemID",
                keyValue: 16,
                column: "Rating",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "ItemID",
                keyValue: 17,
                column: "Rating",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "ItemID",
                keyValue: 18,
                column: "Rating",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "ItemID",
                keyValue: 19,
                column: "Rating",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "ItemID",
                keyValue: 20,
                column: "Rating",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "ItemID",
                keyValue: 21,
                column: "Rating",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "ItemID",
                keyValue: 22,
                column: "Rating",
                value: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Menus");
        }
    }
}
