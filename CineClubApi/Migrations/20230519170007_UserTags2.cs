using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CineClubApi.Migrations
{
    public partial class UserTags2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ListTags_Users_UserId",
                table: "ListTags");

            migrationBuilder.DropIndex(
                name: "IX_ListTags_UserId",
                table: "ListTags");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ListTags");

            migrationBuilder.CreateIndex(
                name: "IX_ListTags_CreatorId",
                table: "ListTags",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_ListTags_Users_CreatorId",
                table: "ListTags",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ListTags_Users_CreatorId",
                table: "ListTags");

            migrationBuilder.DropIndex(
                name: "IX_ListTags_CreatorId",
                table: "ListTags");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "ListTags",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ListTags_UserId",
                table: "ListTags",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ListTags_Users_UserId",
                table: "ListTags",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
