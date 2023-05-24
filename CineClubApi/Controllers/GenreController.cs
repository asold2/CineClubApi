using CineClubApi.Common.DTOs.List;
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
    public async Task<PaginatedListOfMovies> GetMoviesByGenre([FromBody] List<int> genreIds, [FromQuery]int page, [FromQuery]int start, [FromQuery] int end)
    {
        return await _tmdbGenreService.GetMoviesByGenre(genreIds, page, start, end);
    }


}