using CineClubApi.Common.DTOs.Movies;
using CineClubApi.Services.TmdbGenre;
using Microsoft.AspNetCore.Mvc;
using TMDbLib.Objects.General;

namespace CineClubApi.Controllers;

public class GenreController : CineClubControllerBase
{
    private readonly ITMDBGenreService _tmdbGenreService;

    public GenreController(ITMDBGenreService tmdbGenreService)
    {
        _tmdbGenreService = tmdbGenreService;
    }

    [HttpGet("genres")]
    public async  Task<List<Genre>> GetAllGenres()
    {
        return await _tmdbGenreService.GetAllGenres();
    }

    [HttpPost("movies/genre/")]
    public async Task<List<MovieForListDto>> GetMoviesByGenre([FromBody] List<Genre> genres, [FromQuery]int page, [FromQuery]int start, [FromQuery] int end)
    {
        return await _tmdbGenreService.GetMoviesByGenre(genres, page, start, end);
    }


}