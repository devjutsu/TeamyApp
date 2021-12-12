using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teamy.Server.Data.Migrations
{
    public partial class _002_DateVoteUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "474f9851-041a-4a2a-a081-6502b73fcae3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b019c958-c261-4c9b-a1fe-3382134166e9");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "959c567c-d36c-4c06-bf47-1bc8cb8d4a00", "581be658-b651-4531-97a7-a328e44bc550", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "dd978987-44df-439d-aa0f-7cbfaeb9ad80", "f0e01726-77ef-4017-8d8f-e21850d305b6", "Admin", "ADMIN" });

            migrationBuilder.AddForeignKey(
                name: "FK_DateVotes_AspNetUsers_UserId",
                table: "DateVotes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DateVotes_AspNetUsers_UserId",
                table: "DateVotes");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "959c567c-d36c-4c06-bf47-1bc8cb8d4a00");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dd978987-44df-439d-aa0f-7cbfaeb9ad80");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "474f9851-041a-4a2a-a081-6502b73fcae3", "9c62983a-d29d-4ab7-9a45-c7075f2ae1d4", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b019c958-c261-4c9b-a1fe-3382134166e9", "dc4b2f79-fea5-47cf-aac7-9400c9a03b14", "Admin", "ADMIN" });
        }
    }
}
