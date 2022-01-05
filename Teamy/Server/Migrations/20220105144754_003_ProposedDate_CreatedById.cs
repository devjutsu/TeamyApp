using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teamy.Server.Migrations
{
    public partial class _003_ProposedDate_CreatedById : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "ProposedDates",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "ProposedDates");
        }
    }
}
