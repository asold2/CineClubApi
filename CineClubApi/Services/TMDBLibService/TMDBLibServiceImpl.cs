using AutoMapper;
using CineClubApi.Common.DTOs.Movies;
using CineClubApi.Common.Enums;
using CineClubApi.Models;
using Microsoft.EntityFrameworkCore;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Trending;

namespace CineClubApi.Services.TMDBLibService;

public class TMDBLibServiceImpl : ITMDBLibService
{

    // private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private TMDbClient client = null;
    private HttpClient httpClient = null;

    
    public TMDBLibServiceImpl(IConfiguration configuration, IMapper mapper)
    {
        var tmdbAPIKey = Environment.GetEnvironmentVariable("TMDBapiKey");
        client = new TMDbClient(tmdbAPIKey);
        _mapper = mapper;
        httpClient = new HttpClient();
    }

    public async Task<List<MovieForListDto>> GetMoviesByKeyword(string keyword)
    {
        var result =  client.SearchMovieAsync(keyword, 0, true, 0, null, 0).Result.Results.AsQueryable();
        var movieDtos =  _mapper.ProjectTo<MovieForListDto>(result).ToList();

        movieDtos = await AssignImagesToMovie(movieDtos);
        
        return movieDtos;
    }
    

    public async Task<DetailedMovieDto> getMovieById(int id)
    {
        var result = await client.GetMovieAsync(id);

        byte[] movieBackdrop = await GetMovieImage(result.BackdropPath);
        byte[] poster = await GetMovieImage(result.PosterPath);
        
        
        var movieDto =  _mapper.Map<DetailedMovieDto>(result);
        movieDto.Poster = poster;
        movieDto.Backdrop = movieBackdrop;
        return movieDto;
    }

    public async Task<byte[]> GetMovieImage(string url)
    {
        var url_base = "https://image.tmdb.org/t/p/original";
        var response = await httpClient.GetAsync(url_base + url);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsByteArrayAsync();
            return content;
        }
        else
        {
            return null;
        }
    }

    public async Task<List<MovieForListDto>> GetPopularMovies()
    {
        var popularMovies =  client.GetMoviePopularListAsync().Result.Results.AsQueryable();
        var popularMoviesList = _mapper.ProjectTo<MovieForListDto>(popularMovies).ToList();

        popularMoviesList = await AssignImagesToMovie(popularMoviesList);
        
        return popularMoviesList;
    }
    
    public async Task<List<MovieForListDto>> GetTopRatedMovies()
    {
        var topRatedMovies =  client.GetMovieTopRatedListAsync().Result.Results.AsQueryable();
        var topRatedMoviesList = _mapper.ProjectTo<MovieForListDto>(topRatedMovies).ToList();

        topRatedMoviesList = await AssignImagesToMovie(topRatedMoviesList);
        
        return topRatedMoviesList;
    }
    
    public async Task<List<MovieForListDto>> GetUpcomingMovies()
    {
        var upcomingMovies =  client.GetMovieUpcomingListAsync().Result.Results.AsQueryable();
        var upcomingMoviesList = _mapper.ProjectTo<MovieForListDto>(upcomingMovies).ToList();

        upcomingMoviesList = await AssignImagesToMovie(upcomingMoviesList);
        
        return upcomingMoviesList;
    }
    public async Task<List<MovieForListDto>> GetTrendingMovies(TimePeriod period)
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

        var trendingMovies =  client.GetTrendingMoviesAsync(timeWindow).Result.Results.AsQueryable();
        var trendingMoviesList = _mapper.ProjectTo<MovieForListDto>(trendingMovies).ToList();

        trendingMoviesList = await AssignImagesToMovie(trendingMoviesList);
        
        return trendingMoviesList;
    }
    

    private async Task<List<MovieForListDto>> AssignImagesToMovie(List<MovieForListDto> movies)
    {
        foreach (var movie in movies)
        {
            movie.BackdropImage = await GetMovieImage(movie.BackdropPath);
            movie.PosterImage = await GetMovieImage(movie.PosterPath);
        }

        return movies;
    }

}