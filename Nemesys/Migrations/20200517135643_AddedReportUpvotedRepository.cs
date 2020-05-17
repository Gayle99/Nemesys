using Microsoft.EntityFrameworkCore.Migrations;

namespace Nemesys.Migrations
{
    public partial class AddedReportUpvotedRepository : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Reports_ReportId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ReportId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "InvestigatorDetails",
                table: "Investigations");

            migrationBuilder.DropColumn(
                name: "InvestigatorEmail",
                table: "Investigations");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "InvestigatorId",
                table: "Investigations",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ReportUpvoted",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportId = table.Column<int>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportUpvoted", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportUpvoted_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReportUpvoted_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Investigations_InvestigatorId",
                table: "Investigations",
                column: "InvestigatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportUpvoted_ReportId",
                table: "ReportUpvoted",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportUpvoted_UserId",
                table: "ReportUpvoted",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Investigations_AspNetUsers_InvestigatorId",
                table: "Investigations",
                column: "InvestigatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Investigations_AspNetUsers_InvestigatorId",
                table: "Investigations");

            migrationBuilder.DropTable(
                name: "ReportUpvoted");

            migrationBuilder.DropIndex(
                name: "IX_Investigations_InvestigatorId",
                table: "Investigations");

            migrationBuilder.DropColumn(
                name: "InvestigatorId",
                table: "Investigations");

            migrationBuilder.AddColumn<string>(
                name: "InvestigatorDetails",
                table: "Investigations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvestigatorEmail",
                table: "Investigations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReportId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

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
        }
    }
}
