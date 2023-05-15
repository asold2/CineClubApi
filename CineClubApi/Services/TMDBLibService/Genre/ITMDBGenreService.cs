using CineClubApi.Common.DTOs.Movies;
using TMDbLib.Objects.General;

namespace CineClubApi.Services.TmdbGenre;

public interface ITMDBGenreService
{
    Task<List<Genre>> GetAllGenres();
    Task<List<MovieForListDto>> GetMoviesByGenre(List<Genre> genres, int page, int start, int end);
}