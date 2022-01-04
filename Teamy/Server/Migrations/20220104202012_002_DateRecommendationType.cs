using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teamy.Server.Migrations
{
    public partial class _002_DateRecommendationType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "QuizCompletions",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DateRecommendationType",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_QuizQuestions_Id",
                table: "QuizQuestions",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_QuizCompletions_UserId",
                table: "QuizCompletions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizChoices_Id",
                table: "QuizChoices",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Quiz_Id",
                table: "Quiz",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_QuizQuestions_Id",
                table: "QuizQuestions");

            migrationBuilder.DropIndex(
                name: "IX_QuizCompletions_UserId",
                table: "QuizCompletions");

            migrationBuilder.DropIndex(
                name: "IX_QuizChoices_Id",
                table: "QuizChoices");

            migrationBuilder.DropIndex(
                name: "IX_Quiz_Id",
                table: "Quiz");

            migrationBuilder.DropColumn(
                name: "DateRecommendationType",
                table: "Events");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "QuizCompletions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
