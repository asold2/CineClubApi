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

    public async Task<List<MovieForListDto>> GetPopularMovies(int page, int? start, int? end)
    {

        var popularMoviesList = new List<MovieForListDto>();
        var popularMovies = await client.GetMoviePopularListAsync(null, page, null);

        if (start != null && end != null)
        {
            popularMoviesList = await _paginator.PaginateMoviesList(popularMovies, start, page, end);
        }
        else
        {
            popularMoviesList = _mapper.ProjectTo<MovieForListDto>(popularMovies.Results.AsQueryable()).ToList();
        }

        // popularMoviesList = await AssignImagesToMovie(popularMoviesList, false);

        return popularMoviesList;
    }
    
    public async Task<List<MovieForListDto>> GetTopRatedMovies(int page, int? start, int? end)
    {
        var topRatedMoviesList = new List<MovieForListDto>();
        
        var topRatedMovies = await client.GetMovieTopRatedListAsync(null, page, null);

        if (start != null && end != null)
        {
            topRatedMoviesList = await _paginator.PaginateMoviesList(topRatedMovies, start, page, end);
        }
        else
        {
            topRatedMoviesList = _mapper.ProjectTo<MovieForListDto>(topRatedMovies.Results.AsQueryable()).ToList();
        }
        
        // topRatedMoviesList = await AssignImagesToMovie(topRatedMoviesList, false);
        
        return topRatedMoviesList;
    }
    
    public async Task<List<MovieForListDto>> GetUpcomingMovies(int page, int? start, int? end)
    {
        var upcomingMoviesList = new List<MovieForListDto>();
        var upcomingMovies = await  client.GetMovieUpcomingListAsync(null, page, null);

        if (start != null && end != null)
        {
            upcomingMoviesList = await _paginator.PaginateMoviesList(upcomingMovies, start, page, end);
        }
        else
        {
            upcomingMoviesList = _mapper.ProjectTo<MovieForListDto>(upcomingMovies.Results.AsQueryable()).ToList();
        }

        // upcomingMoviesList = await AssignImagesToMovie(upcomingMoviesList, false);
        
        return upcomingMoviesList;
    }
    public async Task<List<MovieForListDto>> GetTrendingMovies(TimePeriod period, int page, int? start, int? end)
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

        var trendingMoviesList = new List<MovieForListDto>();
        var trendingMovies =  await client.GetTrendingMoviesAsync(timeWindow, page);
        
        if (start != null && end != null)
        {
            trendingMoviesList = await _paginator.PaginateMoviesList(trendingMovies, start, page, end);
        }
        else
        {
            trendingMoviesList = _mapper.ProjectTo<MovieForListDto>(trendingMovies.Results.AsQueryable()).ToList();
        }

        // trendingMoviesList = await AssignImagesToMovie(trendingMoviesList, false);
        
        return trendingMoviesList;
    }
}