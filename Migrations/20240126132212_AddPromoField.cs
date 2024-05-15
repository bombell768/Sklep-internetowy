using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lista10_v2.Migrations
{
    public partial class AddPromoField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Promo",
                table: "Article",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Promo",
                table: "Article");
        }
    }
}
