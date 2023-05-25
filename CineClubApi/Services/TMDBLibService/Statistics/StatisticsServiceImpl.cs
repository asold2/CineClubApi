using AutoMapper;
using CineClubApi.Common.DTOs.Genre;
using CineClubApi.Common.DTOs.Movies;
using CineClubApi.Common.Helpers;
using TMDbLib.Objects.Discover;
using TMDbLib.Objects.General;

namespace CineClubApi.Services.TMDBLibService.Statistics;

public class StatisticsServiceImpl :TmdbLib, IStatisticsService
{

    private readonly ITMDBMovieService _tmdbMovieService;
    
    public StatisticsServiceImpl(IMapper mapper, IPaginator paginator, ITMDBMovieService tmdbMovieService) : base(mapper, paginator)
    {
        _tmdbMovieService = tmdbMovieService;
    }

    public async Task<List<MovieForListDto>> TopGrossingMoviesOfAllTime()
    {

        var topGrossingMovies = await client.DiscoverMoviesAsync()
            .OrderBy(DiscoverMovieSortBy.RevenueDesc)
            .Query();

        var movieDtos = _mapper.ProjectTo<MovieForListDto>(topGrossingMovies.Results.Take(10).AsQueryable()).ToList();

        foreach (var movieDto in movieDtos)
        {
            var detailedMovie = await _tmdbMovieService.getMovieById(movieDto.Id);

            movieDto.Budget = detailedMovie.Budget;

        }
        
        return movieDtos;
    }

    public Task<List<RatingGenreDto>> GetAverageRatingByGenre()
    {
        throw new NotImplementedException();
    }
}