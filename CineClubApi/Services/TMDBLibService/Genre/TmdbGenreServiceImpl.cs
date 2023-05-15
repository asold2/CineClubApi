using AutoMapper;
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

    public async Task<List<MovieForListDto>> GetMoviesByGenre(List<Genre> genres, int page, int start, int end)
    {
        var discoverer = client.DiscoverMoviesAsync();


        var moviesByGenre =await discoverer.IncludeWithAllOfGenre(genres).Query();
        
        var moviesByGenreList = await _paginator.PaginateMoviesList(moviesByGenre, start, page, end);
        moviesByGenreList = await AssignImagesToMovie(moviesByGenreList);

        return moviesByGenreList;
        
    }
}