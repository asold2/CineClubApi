using AutoMapper;
using CineClubApi.Common.DTOs.List;
using CineClubApi.Common.DTOs.Movies;
using CineClubApi.Common.Helpers;
using CineClubApi.Services.TMDBLibService;
using TMDbLib.Objects.General;

namespace CineClubApi.Services.TmdbGenre;

public class TmdbGenreServiceImpl: TmdbLib, ITMDBGenreService
{
    
    public TmdbGenreServiceImpl(IMapper mapper, IPaginator paginator) : base(mapper, paginator)
    {
    }
    

    public async Task<List<Genre>> GetAllGenres()
    {
        return await client.GetMovieGenresAsync();
    }

    public async Task<PaginatedListOfMovies> GetMoviesByGenre(List<int> genreIds, int page, int start, int end)
    {
        var discoverer = client.DiscoverMoviesAsync();
        
        var moviesByGenre =await discoverer.IncludeWithAllOfGenre(genreIds).Query();

        var moviesByGenreList = await _paginator.PaginateMoviesList(moviesByGenre, start, page, end);
        // moviesByGenreList = await AssignImagesToMovie(moviesByGenreList, false);

        var paginatedList = new PaginatedListOfMovies
        {
            Movies = moviesByGenreList,
            numberOfPages = moviesByGenre.TotalPages - page
        };
        
        
        return paginatedList;
        
    }

    public async Task<Genre> GetGenreById(int genreId)
    {
        var genres = await client.GetMovieGenresAsync();

        var result = genres.FirstOrDefault(x => x.Id == genreId);

        return result;

    }
}