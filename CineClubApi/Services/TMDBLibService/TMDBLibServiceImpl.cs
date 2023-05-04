using CineClubApi.Models;
using TMDbLib.Client;
using TMDbLib.Objects.Movies;

namespace CineClubApi.Services.TMDBLibService;

public class TMDBLibServiceImpl : ITMDBLibService
{

    private readonly IConfiguration _configuration;
    private TMDbClient client = null;

    
    public TMDBLibServiceImpl(IConfiguration configuration)
    {
        _configuration = configuration;
        client = new TMDbClient(_configuration.GetConnectionString("TMDBapiKey"));
    }


    public async Task getAllMovies()
    {
        Movie movie = await client.GetMovieAsync(47964);
        Console.WriteLine(movie);
        
    }

    public async Task<MovieDao> GetMovieByKeyword(string keyword)
    {
        var result = await client.SearchMovieAsync(keyword, 0, true, 0, null, 0);

        return null;

    }
}