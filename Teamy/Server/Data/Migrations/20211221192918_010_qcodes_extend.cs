using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teamy.Server.Data.Migrations
{
    public partial class _010_qcodes_extend : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuizCompletions_Quiz_QuizId",
                table: "QuizCompletions");

            migrationBuilder.DropIndex(
                name: "IX_QuizCompletions_QuizId",
                table: "QuizCompletions");

            migrationBuilder.DropColumn(
                name: "QuizId",
                table: "QuizCompletions");

            migrationBuilder.AddColumn<string>(
                name: "QCodeId",
                table: "QuizCompletions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "QCodes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "QCodes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_QuizCompletions_Id",
                table: "QuizCompletions",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_QuizCompletions_QCodeId",
                table: "QuizCompletions",
                column: "QCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizAnswers_Id",
                table: "QuizAnswers",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_QCodes_Id",
                table: "QCodes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizCompletions_QCodes_QCodeId",
                table: "QuizCompletions",
                column: "QCodeId",
                principalTable: "QCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuizCompletions_QCodes_QCodeId",
                table: "QuizCompletions");

            migrationBuilder.DropIndex(
                name: "IX_QuizCompletions_Id",
                table: "QuizCompletions");

            migrationBuilder.DropIndex(
                name: "IX_QuizCompletions_QCodeId",
                table: "QuizCompletions");

            migrationBuilder.DropIndex(
                name: "IX_QuizAnswers_Id",
                table: "QuizAnswers");

            migrationBuilder.DropIndex(
                name: "IX_QCodes_Id",
                table: "QCodes");

            migrationBuilder.DropColumn(
                name: "QCodeId",
                table: "QuizCompletions");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "QCodes");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "QCodes");

            migrationBuilder.AddColumn<Guid>(
                name: "QuizId",
                table: "QuizCompletions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_QuizCompletions_QuizId",
                table: "QuizCompletions",
                column: "QuizId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizCompletions_Quiz_QuizId",
                table: "QuizCompletions",
                column: "QuizId",
                principalTable: "Quiz",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
