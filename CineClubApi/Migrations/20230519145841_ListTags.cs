using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CineClubApi.Migrations
{
    public partial class ListTags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ListTags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListTags", x => x.Id);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListListTag");

            migrationBuilder.DropTable(
                name: "ListTags");
        }
    }
}
