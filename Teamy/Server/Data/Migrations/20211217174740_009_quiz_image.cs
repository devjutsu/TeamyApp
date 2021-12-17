using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teamy.Server.Data.Migrations
{
    public partial class _009_quiz_image : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Details",
                table: "Quiz",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ImageId",
                table: "Quiz",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Quiz_ImageId",
                table: "Quiz",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quiz_Images_ImageId",
                table: "Quiz",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quiz_Images_ImageId",
                table: "Quiz");

            migrationBuilder.DropIndex(
                name: "IX_Quiz_ImageId",
                table: "Quiz");

            migrationBuilder.DropColumn(
                name: "Details",
                table: "Quiz");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Quiz");
        }
    }
}
