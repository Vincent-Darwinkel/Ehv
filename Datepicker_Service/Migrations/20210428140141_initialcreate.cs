using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Datepicker_Service.Migrations
{
    public partial class initialcreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Datepicker",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(nullable: false),
                    AuthorUuid = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    Expires = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Datepicker", x => x.Uuid);
                });

            migrationBuilder.CreateTable(
                name: "DatepickerDate",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(nullable: false),
                    DatePickerUuid = table.Column<Guid>(nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false),
                    DatepickerDtoUuid = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatepickerDate", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_DatepickerDate_Datepicker_DatepickerDtoUuid",
                        column: x => x.DatepickerDtoUuid,
                        principalTable: "Datepicker",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DatepickerAvailabilityDto",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(nullable: false),
                    DateUuid = table.Column<Guid>(nullable: false),
                    UserUuid = table.Column<Guid>(nullable: false),
                    DatepickerDateDtoUuid = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatepickerAvailabilityDto", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_DatepickerAvailabilityDto_DatepickerDate_DatepickerDateDtoUu~",
                        column: x => x.DatepickerDateDtoUuid,
                        principalTable: "DatepickerDate",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DatepickerAvailabilityDto_DatepickerDateDtoUuid",
                table: "DatepickerAvailabilityDto",
                column: "DatepickerDateDtoUuid");

            migrationBuilder.CreateIndex(
                name: "IX_DatepickerDate_DatepickerDtoUuid",
                table: "DatepickerDate",
                column: "DatepickerDtoUuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DatepickerAvailabilityDto");

            migrationBuilder.DropTable(
                name: "DatepickerDate");

            migrationBuilder.DropTable(
                name: "Datepicker");
        }
    }
}
