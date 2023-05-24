using CineClubApi.Common.DTOs.List;
using CineClubApi.Common.DTOs.Movies;
using TMDbLib.Objects.General;

namespace CineClubApi.Services.TmdbGenre;

public interface ITMDBGenreService
{
    Task<List<Genre>> GetAllGenres();
    Task<PaginatedListOfMovies> GetMoviesByGenre(List<int> genreIds , int page, int start, int end);
}