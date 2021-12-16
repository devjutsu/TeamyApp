using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teamy.Server.Data.Migrations
{
    public partial class _005_quiz_title : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Quiz",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Quiz");
        }
    }
}
