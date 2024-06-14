using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAcessLayer.Migrations
{
    public partial class addedmealName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealMenus_MealPlans_MealPlanId",
                table: "MealMenus");

            migrationBuilder.DropForeignKey(
                name: "FK_MealPlans_Foods_FoodId",
                table: "MealPlans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MealPlans",
                table: "MealPlans");

            migrationBuilder.DropColumn(
                name: "MealName",
                table: "MealPlans");

            migrationBuilder.DropColumn(
                name: "MealType",
                table: "MealPlans");

            migrationBuilder.RenameTable(
                name: "MealPlans",
                newName: "Meals");

            migrationBuilder.RenameColumn(
                name: "MealPlanId",
                table: "MealMenus",
                newName: "MealNameId");

            migrationBuilder.RenameIndex(
                name: "IX_MealMenus_MealPlanId",
                table: "MealMenus",
                newName: "IX_MealMenus_MealNameId");

            migrationBuilder.RenameIndex(
                name: "IX_MealPlans_FoodId",
                table: "Meals",
                newName: "IX_Meals_FoodId");

            migrationBuilder.AddColumn<int>(
                name: "MealNameId",
                table: "Meals",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Meals",
                table: "Meals",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "MealNames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    MealType = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealNames", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Meals_MealNameId",
                table: "Meals",
                column: "MealNameId");

            migrationBuilder.AddForeignKey(
                name: "FK_MealMenus_MealNames_MealNameId",
                table: "MealMenus",
                column: "MealNameId",
                principalTable: "MealNames",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Meals_Foods_FoodId",
                table: "Meals",
                column: "FoodId",
                principalTable: "Foods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Meals_MealNames_MealNameId",
                table: "Meals",
                column: "MealNameId",
                principalTable: "MealNames",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealMenus_MealNames_MealNameId",
                table: "MealMenus");

            migrationBuilder.DropForeignKey(
                name: "FK_Meals_Foods_FoodId",
                table: "Meals");

            migrationBuilder.DropForeignKey(
                name: "FK_Meals_MealNames_MealNameId",
                table: "Meals");

            migrationBuilder.DropTable(
                name: "MealNames");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Meals",
                table: "Meals");

            migrationBuilder.DropIndex(
                name: "IX_Meals_MealNameId",
                table: "Meals");

            migrationBuilder.DropColumn(
                name: "MealNameId",
                table: "Meals");

            migrationBuilder.RenameTable(
                name: "Meals",
                newName: "MealPlans");

            migrationBuilder.RenameColumn(
                name: "MealNameId",
                table: "MealMenus",
                newName: "MealPlanId");

            migrationBuilder.RenameIndex(
                name: "IX_MealMenus_MealNameId",
                table: "MealMenus",
                newName: "IX_MealMenus_MealPlanId");

            migrationBuilder.RenameIndex(
                name: "IX_Meals_FoodId",
                table: "MealPlans",
                newName: "IX_MealPlans_FoodId");

            migrationBuilder.AddColumn<string>(
                name: "MealName",
                table: "MealPlans",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MealType",
                table: "MealPlans",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MealPlans",
                table: "MealPlans",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MealMenus_MealPlans_MealPlanId",
                table: "MealMenus",
                column: "MealPlanId",
                principalTable: "MealPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MealPlans_Foods_FoodId",
                table: "MealPlans",
                column: "FoodId",
                principalTable: "Foods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
