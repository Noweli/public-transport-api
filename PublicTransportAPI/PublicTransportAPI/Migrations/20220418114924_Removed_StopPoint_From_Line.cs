using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PublicTransportAPI.Migrations
{
    public partial class Removed_StopPoint_From_Line : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StopPoints_Lines_LineId",
                table: "StopPoints");

            migrationBuilder.DropIndex(
                name: "IX_StopPoints_LineId",
                table: "StopPoints");

            migrationBuilder.DropColumn(
                name: "LineId",
                table: "StopPoints");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LineId",
                table: "StopPoints",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StopPoints_LineId",
                table: "StopPoints",
                column: "LineId");

            migrationBuilder.AddForeignKey(
                name: "FK_StopPoints_Lines_LineId",
                table: "StopPoints",
                column: "LineId",
                principalTable: "Lines",
                principalColumn: "Id");
        }
    }
}
