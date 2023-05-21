using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CineClubApi.Migrations
{
    public partial class UserTags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListListTag");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "ListTags",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "ListTags",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ListTag",
                columns: table => new
                {
                    ListsId = table.Column<Guid>(type: "uuid", nullable: false),
                    TagsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListTag", x => new { x.ListsId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_ListTag_Lists_ListsId",
                        column: x => x.ListsId,
                        principalTable: "Lists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ListTag_ListTags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "ListTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ListTags_UserId",
                table: "ListTags",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ListTag_TagsId",
                table: "ListTag",
                column: "TagsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ListTags_Users_UserId",
                table: "ListTags",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ListTags_Users_UserId",
                table: "ListTags");

            migrationBuilder.DropTable(
                name: "ListTag");

            migrationBuilder.DropIndex(
                name: "IX_ListTags_UserId",
                table: "ListTags");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "ListTags");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ListTags");

            migrationBuilder.CreateTable(
                name: "ListListTag",
                columns: table => new
                {
                    ListsId = table.Column<Guid>(type: "uuid", nullable: false),
                    TagsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListListTag", x => new { x.ListsId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_ListListTag_Lists_ListsId",
                        column: x => x.ListsId,
                        principalTable: "Lists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ListListTag_ListTags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "ListTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ListListTag_TagsId",
                table: "ListListTag",
                column: "TagsId");
        }
    }
}
