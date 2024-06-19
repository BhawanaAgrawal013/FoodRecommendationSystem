using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAcessLayer.Migrations
{
    public partial class fixingUserNotification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.Sql("SET IDENTITY_INSERT dbo.UserNotifications OFF"); // Turn off IDENTITY temporarily

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserNotifications");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserNotifications",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.Sql("SET IDENTITY_INSERT dbo.UserNotifications ON"); // Turn on IDENTITY back
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.Sql("SET IDENTITY_INSERT dbo.UserNotifications OFF"); // Turn off IDENTITY temporarily

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserNotifications");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserNotifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql("SET IDENTITY_INSERT dbo.UserNotifications ON"); // Turn on IDENTITY back
        }

    }
}
