using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce_Mvc.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureProductRelationsAndSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "CreatedAtUtc", "Description", "Name", "PictureUri", "Price", "Quantity", "SellerId", "UserId" },
                values: new object[] { 1, 1, new DateTime(2023, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), "High performance laptop", "Laptop", "laptop.png", 15000m, 10, "a6f76fb7-5b39-4ecb-9639-f3bba5117dd5", "a6f76fb7-5b39-4ecb-9639-f3bba5117dd5" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
