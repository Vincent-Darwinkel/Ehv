using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Event_Service.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AuthorUuid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Title = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Location = table.Column<string>(type: "varchar(125)", maxLength: 125, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.Uuid);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EventDate",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EventUuid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventDate", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_EventDate_Event_EventUuid",
                        column: x => x.EventUuid,
                        principalTable: "Event",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EventStep",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    StepNr = table.Column<int>(type: "int", nullable: false),
                    EventUuid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Text = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventStep", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_EventStep_Event_EventUuid",
                        column: x => x.EventUuid,
                        principalTable: "Event",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EventDateUser",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EventDateUuid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserUuid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventDateUser", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_EventDateUser_EventDate_EventDateUuid",
                        column: x => x.EventDateUuid,
                        principalTable: "EventDate",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EventStepUser",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EventStepUuid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserUuid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventStepUser", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_EventStepUser_EventStep_EventStepUuid",
                        column: x => x.EventStepUuid,
                        principalTable: "EventStep",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_EventDate_EventUuid",
                table: "EventDate",
                column: "EventUuid");

            migrationBuilder.CreateIndex(
                name: "IX_EventDateUser_EventDateUuid",
                table: "EventDateUser",
                column: "EventDateUuid");

            migrationBuilder.CreateIndex(
                name: "IX_EventStep_EventUuid",
                table: "EventStep",
                column: "EventUuid");

            migrationBuilder.CreateIndex(
                name: "IX_EventStepUser_EventStepUuid",
                table: "EventStepUser",
                column: "EventStepUuid");
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
