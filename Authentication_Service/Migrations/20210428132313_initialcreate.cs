using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Authentication_Service.Migrations
{
    public partial class initialcreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Activation",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(nullable: false),
                    UserUuid = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activation", x => x.Uuid);
                });

            migrationBuilder.CreateTable(
                name: "DisabledUser",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(nullable: false),
                    UserUuid = table.Column<Guid>(nullable: false),
                    Reason = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisabledUser", x => x.Uuid);
                });

            migrationBuilder.CreateTable(
                name: "PasswordReset",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(nullable: false),
                    UserUuid = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordReset", x => x.Uuid);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    UserUuid = table.Column<Guid>(nullable: false),
                    RefreshToken = table.Column<string>(nullable: true),
                    ExpirationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.UserUuid);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    AccountRole = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Uuid);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activation");

            migrationBuilder.DropTable(
                name: "DisabledUser");

            migrationBuilder.DropTable(
                name: "PasswordReset");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
