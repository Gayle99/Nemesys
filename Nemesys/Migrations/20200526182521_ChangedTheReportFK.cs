using Microsoft.EntityFrameworkCore.Migrations;

namespace Nemesys.Migrations
{
    public partial class ChangedTheReportFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "Investigations");

            migrationBuilder.AddColumn<int>(
                name: "AssociatedReportId",
                table: "Investigations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Investigations_AssociatedReportId",
                table: "Investigations",
                column: "AssociatedReportId");

            migrationBuilder.AddForeignKey(
                name: "FK_Investigations_Reports_AssociatedReportId",
                table: "Investigations",
                column: "AssociatedReportId",
                principalTable: "Reports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Investigations_Reports_AssociatedReportId",
                table: "Investigations");

            migrationBuilder.DropIndex(
                name: "IX_Investigations_AssociatedReportId",
                table: "Investigations");

            migrationBuilder.DropColumn(
                name: "AssociatedReportId",
                table: "Investigations");

            migrationBuilder.AddColumn<int>(
                name: "ReportId",
                table: "Investigations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
