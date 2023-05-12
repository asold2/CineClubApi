using AutoMapper;
using CineClubApi.Common.DTOs.Movies;
using CineClubApi.Models;
using Microsoft.EntityFrameworkCore;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;

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
        // _configuration = configuration;
        client = new TMDbClient(tmdbAPIKey);
        _mapper = mapper;
        httpClient = new HttpClient();
    }

    //returns list of movies based on the provided name
    public async Task<List<MovieForListDto>> GetMoviesByKeyword(string keyword)
    {
        var result =  client.SearchMovieAsync(keyword, 0, true, 0, null, 0).Result.Results.AsQueryable();
        var movieDtos =  _mapper.ProjectTo<MovieForListDto>(result).ToList();
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

}