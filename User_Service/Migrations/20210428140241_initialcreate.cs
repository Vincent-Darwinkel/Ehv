using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace User_Service.Migrations
{
    public partial class initialcreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    About = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Gender = table.Column<int>(nullable: false),
                    AccountRole = table.Column<int>(nullable: false),
                    BirthDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Uuid);
                });

            migrationBuilder.CreateTable(
                name: "Artist",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(nullable: false),
                    UserUuid = table.Column<Guid>(nullable: false),
                    Artist = table.Column<string>(nullable: true),
                    UserDtoUuid = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artist", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_Artist_User_UserDtoUuid",
                        column: x => x.UserDtoUuid,
                        principalTable: "User",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Hobby",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(nullable: false),
                    UserUuid = table.Column<Guid>(nullable: false),
                    Hobby = table.Column<string>(nullable: true),
                    UserDtoUuid = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hobby", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_Hobby_User_UserDtoUuid",
                        column: x => x.UserDtoUuid,
                        principalTable: "User",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Artist_UserDtoUuid",
                table: "Artist",
                column: "UserDtoUuid");

            migrationBuilder.CreateIndex(
                name: "IX_Hobby_UserDtoUuid",
                table: "Hobby",
                column: "UserDtoUuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Artist");

            migrationBuilder.DropTable(
                name: "Hobby");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
