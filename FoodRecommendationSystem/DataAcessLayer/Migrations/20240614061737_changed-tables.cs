using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAcessLayer.Migrations
{
    public partial class changedtables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Appearances_AppearanceId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Qualities_QualityId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Quantities_QuantityId",
                table: "Reviews");

            migrationBuilder.DropTable(
                name: "Appearances");

            migrationBuilder.DropTable(
                name: "Qualities");

            migrationBuilder.DropTable(
                name: "Quantities");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_AppearanceId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_QualityId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_QuantityId",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "QuantityId",
                table: "Reviews",
                newName: "ValueForMoneyRating");

            migrationBuilder.RenameColumn(
                name: "QualityId",
                table: "Reviews",
                newName: "QuantityRating");

            migrationBuilder.RenameColumn(
                name: "AppearanceId",
                table: "Reviews",
                newName: "QualityRating");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfPeople",
                table: "SummaryRatings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "TotalValueForMoneyRating",
                table: "SummaryRatings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "AppearanceRating",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MealType",
                table: "MealPlans",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MealMenus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MealPlanId = table.Column<int>(type: "int", nullable: false),
                    NumberOfVotes = table.Column<int>(type: "int", nullable: false),
                    WasPrepared = table.Column<bool>(type: "bit", nullable: false),
                    Classification = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealMenus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MealMenus_MealPlans_MealPlanId",
                        column: x => x.MealPlanId,
                        principalTable: "MealPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotificationMessage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserNotification",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    NotificationId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserNotification", x => new { x.UserId, x.NotificationId });
                    table.ForeignKey(
                        name: "FK_UserNotification_Notifications_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserNotification_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MealMenus_MealPlanId",
                table: "MealMenus",
                column: "MealPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_UserNotification_NotificationId",
                table: "UserNotification",
                column: "NotificationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MealMenus");

            migrationBuilder.DropTable(
                name: "UserNotification");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropColumn(
                name: "NumberOfPeople",
                table: "SummaryRatings");

            migrationBuilder.DropColumn(
                name: "TotalValueForMoneyRating",
                table: "SummaryRatings");

            migrationBuilder.DropColumn(
                name: "AppearanceRating",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "MealType",
                table: "MealPlans");

            migrationBuilder.RenameColumn(
                name: "ValueForMoneyRating",
                table: "Reviews",
                newName: "QuantityId");

            migrationBuilder.RenameColumn(
                name: "QuantityRating",
                table: "Reviews",
                newName: "QualityId");

            migrationBuilder.RenameColumn(
                name: "QualityRating",
                table: "Reviews",
                newName: "AppearanceId");

            migrationBuilder.CreateTable(
                name: "Appearances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppearanceValue = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appearances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Qualities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QualityValue = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Qualities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Quantities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuantityValue = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quantities", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_AppearanceId",
                table: "Reviews",
                column: "AppearanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_QualityId",
                table: "Reviews",
                column: "QualityId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_QuantityId",
                table: "Reviews",
                column: "QuantityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Appearances_AppearanceId",
                table: "Reviews",
                column: "AppearanceId",
                principalTable: "Appearances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Qualities_QualityId",
                table: "Reviews",
                column: "QualityId",
                principalTable: "Qualities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Quantities_QuantityId",
                table: "Reviews",
                column: "QuantityId",
                principalTable: "Quantities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
