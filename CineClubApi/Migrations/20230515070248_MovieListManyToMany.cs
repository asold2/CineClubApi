using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CineClubApi.Migrations
{
    public partial class MovieListManyToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MovieDaos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    tmdbId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieDaos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ListMovieDao",
                columns: table => new
                {
                    ListsId = table.Column<Guid>(type: "uuid", nullable: false),
                    MovieDaosId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListMovieDao", x => new { x.ListsId, x.MovieDaosId });
                    table.ForeignKey(
                        name: "FK_ListMovieDao_Lists_ListsId",
                        column: x => x.ListsId,
                        principalTable: "Lists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ListMovieDao_MovieDaos_MovieDaosId",
                        column: x => x.MovieDaosId,
                        principalTable: "MovieDaos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ListMovieDao_MovieDaosId",
                table: "ListMovieDao",
                column: "MovieDaosId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListMovieDao");

            migrationBuilder.DropTable(
                name: "MovieDaos");
        }
    }
}
