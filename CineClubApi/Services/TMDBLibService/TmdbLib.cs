using AutoMapper;
using CineClubApi.Common.DTOs.Movies;
using CineClubApi.Common.Helpers;
using TMDbLib.Client;

namespace CineClubApi.Services.TMDBLibService;

public abstract class TmdbLib
{
    protected readonly IMapper _mapper;
    protected readonly TMDbClient client;
    protected readonly HttpClient httpClient;
    protected readonly IPaginator _paginator;
    
    
    public TmdbLib( IMapper mapper, IPaginator paginator)
    {
        client = new TMDbClient(Environment.GetEnvironmentVariable("TMDBapiKey"));
        _mapper = mapper;
        httpClient = new HttpClient();
        _paginator = paginator;
    }
    
    
    public async Task<byte[]> GetImageFromPath(string url)
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


    protected async Task<List<MovieForListDto>> AssignImagesToMovie(List<MovieForListDto> movies, bool both)
    {

        if (both)
        {
            foreach (var movie in movies)
            {
                movie.BackdropImage = await GetImageFromPath(movie.BackdropPath);
                movie.PosterImage = await GetImageFromPath(movie.PosterPath);
            }
        }
        else
        {
            foreach (var movie in movies)
            {
                // movie.BackdropImage = await GetMovieImage(movie.BackdropPath);
                movie.PosterImage = await GetImageFromPath(movie.PosterPath);
            }
        }

        return movies;
    }
}