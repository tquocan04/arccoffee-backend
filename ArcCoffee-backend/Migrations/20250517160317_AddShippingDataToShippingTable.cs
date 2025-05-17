using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ArcCoffee_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddShippingDataToShippingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ShippingMethods",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { "BE", "BE" },
                    { "GRAB", "GRAB" },
                    { "SHOPEEFOOD", "SHOPEE FOOD" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ShippingMethods",
                keyColumn: "Id",
                keyValue: "BE");

            migrationBuilder.DeleteData(
                table: "ShippingMethods",
                keyColumn: "Id",
                keyValue: "GRAB");

            migrationBuilder.DeleteData(
                table: "ShippingMethods",
                keyColumn: "Id",
                keyValue: "SHOPEEFOOD");
        }
    }
}
