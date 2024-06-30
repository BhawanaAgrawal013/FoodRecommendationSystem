using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAcessLayer.Migrations
{
    public partial class complexitiesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Meals",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CuisinePreference",
                table: "MealNames",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DietType",
                table: "MealNames",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSweet",
                table: "MealNames",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SpiceLevel",
                table: "MealNames",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DiscardedMenuFeedbacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MealNameId = table.Column<int>(type: "int", nullable: false),
                    IsDiscarded = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscardedMenus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscardedMenus_MealNames_MealNameId",
                        column: x => x.MealNameId,
                        principalTable: "MealNames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DietType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpiceLevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CuisinePreference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsSweet = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Profiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiscardedMenuFeedbacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscardedMenuId = table.Column<int>(type: "int", nullable: false),
                    DislikeText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LikeText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Recipie = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscardedMenuFeedbacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscardedMenuFeedbacks_DiscardedMenus_DiscardedMenuId",
                        column: x => x.DiscardedMenuId,
                        principalTable: "DiscardedMenuFeedbacks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiscardedMenuFeedbacks_DiscardedMenuId",
                table: "DiscardedMenuFeedbacks",
                column: "DiscardedMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscardedMenus_MealNameId",
                table: "DiscardedMenuFeedbacks",
                column: "MealNameId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_UserId",
                table: "Profiles",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiscardedMenuFeedbacks");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropTable(
                name: "DiscardedMenuFeedbacks");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Meals");

            migrationBuilder.DropColumn(
                name: "CuisinePreference",
                table: "MealNames");

            migrationBuilder.DropColumn(
                name: "DietType",
                table: "MealNames");

            migrationBuilder.DropColumn(
                name: "IsSweet",
                table: "MealNames");

            migrationBuilder.DropColumn(
                name: "SpiceLevel",
                table: "MealNames");
        }
    }
}
