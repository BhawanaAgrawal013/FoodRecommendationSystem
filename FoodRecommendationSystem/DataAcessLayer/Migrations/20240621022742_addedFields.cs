using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAcessLayer.Migrations
{
    public partial class addedFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SentimentComment",
                table: "SummaryRatings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "MealNames",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Foods",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SentimentComment",
                table: "SummaryRatings");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "MealNames");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Foods");
        }
    }
}
