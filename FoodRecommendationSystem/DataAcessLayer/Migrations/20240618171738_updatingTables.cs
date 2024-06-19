using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAcessLayer.Migrations
{
    public partial class updatingTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserNotification_Notifications_NotificationId",
                table: "UserNotification");

            migrationBuilder.DropForeignKey(
                name: "FK_UserNotification_Users_UserId",
                table: "UserNotification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserNotification",
                table: "UserNotification");

            migrationBuilder.DropColumn(
                name: "ReviewSummary",
                table: "SummaryRatings");

            migrationBuilder.RenameTable(
                name: "UserNotification",
                newName: "UserNotifications");

            migrationBuilder.RenameIndex(
                name: "IX_UserNotification_NotificationId",
                table: "UserNotifications",
                newName: "IX_UserNotifications_NotificationId");

            migrationBuilder.AddColumn<double>(
                name: "SentimentScore",
                table: "SummaryRatings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "MealMenus",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserNotifications",
                table: "UserNotifications",
                columns: new[] { "UserId", "NotificationId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserNotifications_Notifications_NotificationId",
                table: "UserNotifications",
                column: "NotificationId",
                principalTable: "Notifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserNotifications_Users_UserId",
                table: "UserNotifications",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserNotifications_Notifications_NotificationId",
                table: "UserNotifications");

            migrationBuilder.DropForeignKey(
                name: "FK_UserNotifications_Users_UserId",
                table: "UserNotifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserNotifications",
                table: "UserNotifications");

            migrationBuilder.DropColumn(
                name: "SentimentScore",
                table: "SummaryRatings");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "MealMenus");

            migrationBuilder.RenameTable(
                name: "UserNotifications",
                newName: "UserNotification");

            migrationBuilder.RenameIndex(
                name: "IX_UserNotifications_NotificationId",
                table: "UserNotification",
                newName: "IX_UserNotification_NotificationId");

            migrationBuilder.AddColumn<string>(
                name: "ReviewSummary",
                table: "SummaryRatings",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserNotification",
                table: "UserNotification",
                columns: new[] { "UserId", "NotificationId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserNotification_Notifications_NotificationId",
                table: "UserNotification",
                column: "NotificationId",
                principalTable: "Notifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserNotification_Users_UserId",
                table: "UserNotification",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
