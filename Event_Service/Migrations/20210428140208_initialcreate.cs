using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Event_Service.Migrations
{
    public partial class initialcreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(nullable: false),
                    AuthorUuid = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.Uuid);
                });

            migrationBuilder.CreateTable(
                name: "EventDate",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(nullable: false),
                    EventUuid = table.Column<Guid>(nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false),
                    EventDtoUuid = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventDate", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_EventDate_Event_EventDtoUuid",
                        column: x => x.EventDtoUuid,
                        principalTable: "Event",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EventStep",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(nullable: false),
                    StepNr = table.Column<int>(nullable: false),
                    EventUuid = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    EventDtoUuid = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventStep", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_EventStep_Event_EventDtoUuid",
                        column: x => x.EventDtoUuid,
                        principalTable: "Event",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EventDateUser",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(nullable: false),
                    EventDateUuid = table.Column<Guid>(nullable: false),
                    UserUuid = table.Column<Guid>(nullable: false),
                    EventDateDtoUuid = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventDateUser", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_EventDateUser_EventDate_EventDateDtoUuid",
                        column: x => x.EventDateDtoUuid,
                        principalTable: "EventDate",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EventStepUser",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(nullable: false),
                    EventStepUuid = table.Column<Guid>(nullable: false),
                    UserUuid = table.Column<Guid>(nullable: false),
                    EventStepDtoUuid = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventStepUser", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_EventStepUser_EventStep_EventStepDtoUuid",
                        column: x => x.EventStepDtoUuid,
                        principalTable: "EventStep",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventDate_EventDtoUuid",
                table: "EventDate",
                column: "EventDtoUuid");

            migrationBuilder.CreateIndex(
                name: "IX_EventDateUser_EventDateDtoUuid",
                table: "EventDateUser",
                column: "EventDateDtoUuid");

            migrationBuilder.CreateIndex(
                name: "IX_EventStep_EventDtoUuid",
                table: "EventStep",
                column: "EventDtoUuid");

            migrationBuilder.CreateIndex(
                name: "IX_EventStepUser_EventStepDtoUuid",
                table: "EventStepUser",
                column: "EventStepDtoUuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventDateUser");

            migrationBuilder.DropTable(
                name: "EventStepUser");

            migrationBuilder.DropTable(
                name: "EventDate");

            migrationBuilder.DropTable(
                name: "EventStep");

            migrationBuilder.DropTable(
                name: "Event");
        }
    }
}
