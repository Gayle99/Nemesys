using Microsoft.EntityFrameworkCore.Migrations;

namespace Nemesys.Migrations
{
    public partial class AddedUserRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "Upvotes",
                table: "Reports");

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Reports",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReportId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_CreatedById",
                table: "Reports",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ReportId",
                table: "AspNetUsers",
                column: "ReportId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Reports_ReportId",
                table: "AspNetUsers",
                column: "ReportId",
                principalTable: "Reports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_AspNetUsers_CreatedById",
                table: "Reports",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Reports_ReportId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_AspNetUsers_CreatedById",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_CreatedById",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ReportId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Upvotes",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
