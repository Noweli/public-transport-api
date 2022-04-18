using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PublicTransportAPI.Migrations
{
    public partial class Added_StopPoint_LineEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StopPointLineEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Arrival = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Departure = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LineId = table.Column<int>(type: "INTEGER", nullable: true),
                    StopPointId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StopPointLineEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StopPointLineEvents_Lines_LineId",
                        column: x => x.LineId,
                        principalTable: "Lines",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StopPointLineEvents_StopPoints_StopPointId",
                        column: x => x.StopPointId,
                        principalTable: "StopPoints",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_StopPointLineEvents_LineId",
                table: "StopPointLineEvents",
                column: "LineId");

            migrationBuilder.CreateIndex(
                name: "IX_StopPointLineEvents_StopPointId",
                table: "StopPointLineEvents",
                column: "StopPointId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StopPointLineEvents");
        }
    }
}
