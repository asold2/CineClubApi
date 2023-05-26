using AutoMapper;
using CineClubApi.Common.DTOs.List;
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

    public async Task<PaginatedListOfMovies> GetPopularMovies(int page, int? start, int? end)
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


        var paginatedList = new PaginatedListOfMovies
        {
            Movies = popularMoviesList,
            numberOfPages = popularMovies.TotalPages
        };
 
        return paginatedList;
    }
    
    public async Task<PaginatedListOfMovies> GetTopRatedMovies(int page, int? start, int? end)
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
        
        var paginatedList = new PaginatedListOfMovies
        {
            Movies = topRatedMoviesList,
            numberOfPages = topRatedMovies.TotalPages
        };
 
        return paginatedList;
    }
    
    public async Task<PaginatedListOfMovies> GetUpcomingMovies(int page, int? start, int? end)
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

        var paginatedList = new PaginatedListOfMovies
        {
            Movies = upcomingMoviesList,
            numberOfPages = upcomingMovies.TotalPages 
        };
 
        return paginatedList;
    }
    public async Task<PaginatedListOfMovies> GetTrendingMovies(TimePeriod period, int page, int? start, int? end)
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

        var paginatedList = new PaginatedListOfMovies
        {
            Movies = trendingMoviesList,
            numberOfPages = trendingMovies.TotalPages
        };
 
        return paginatedList;
    }
    
}