using AutoMapper;
using CineClubApi.Common.DTOs.Movies;
using CineClubApi.Common.Enums;
using CineClubApi.Common.Helpers;
using TMDbLib.Objects.Trending;

namespace CineClubApi.Services.TMDBLibService.Lists;

public class TmdbListServiceImpl :TmdbLib, ITmdbListService
{
    public TmdbListServiceImpl( IMapper mapper, IPaginator paginator) : base(mapper, paginator)
    {
    }

    public async Task<List<MovieForListDto>> GetPopularMovies(int page, int start, int end)
    {


        var popularMovies = await client.GetMoviePopularListAsync(null, page, null);

        var popularMoviesList = await _paginator.PaginateMoviesList(popularMovies, start, page, end);
        popularMoviesList = await AssignImagesToMovie(popularMoviesList);

        return popularMoviesList;
    }
    
    public async Task<List<MovieForListDto>> GetTopRatedMovies(int page, int start, int end)
    {
        var topRatedMovies = await client.GetMovieTopRatedListAsync(null, page, null);

        var topRatedMoviesList = await _paginator.PaginateMoviesList(topRatedMovies, start, page, end);

        topRatedMoviesList = await AssignImagesToMovie(topRatedMoviesList);
        
        return topRatedMoviesList;
    }
    
    public async Task<List<MovieForListDto>> GetUpcomingMovies(int page, int start, int end)
    {
        
        var upcomingMovies = await  client.GetMovieUpcomingListAsync(null, page, null);

        var upcomingMoviesList = await _paginator.PaginateMoviesList(upcomingMovies, start, page, end);

        upcomingMoviesList = await AssignImagesToMovie(upcomingMoviesList);
        
        return upcomingMoviesList;
    }
    public async Task<List<MovieForListDto>> GetTrendingMovies(TimePeriod period, int page, int start, int end)
    {
        TimeWindow timeWindow = new TimeWindow();

        switch (period)
        {
            case TimePeriod.Day:
                timeWindow = TimeWindow.Day;
                break;
            case TimePeriod.Week:
                timeWindow = TimeWindow.Week;
                break;
        }


        var trendingMovies =  await client.GetTrendingMoviesAsync(timeWindow, page);
        var trendingMoviesList = await _paginator.PaginateMoviesList(trendingMovies, start, page, end);;

        trendingMoviesList = await AssignImagesToMovie(trendingMoviesList);
        
        return trendingMoviesList;
    }
}