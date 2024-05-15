using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Lista10_v2.Migrations
{
    public partial class AddExampleArticles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Nabiał" });

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Pieczywo" });

            migrationBuilder.InsertData(
                table: "Article",
                columns: new[] { "Id", "CategoryId", "ExpirationDate", "ImagePath", "Name", "Price" },
                values: new object[] { 1, 1, new DateTime(2024, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Masło", 10.0 });

            migrationBuilder.InsertData(
                table: "Article",
                columns: new[] { "Id", "CategoryId", "ExpirationDate", "ImagePath", "Name", "Price" },
                values: new object[] { 2, 2, new DateTime(2023, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Chleb", 5.0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
