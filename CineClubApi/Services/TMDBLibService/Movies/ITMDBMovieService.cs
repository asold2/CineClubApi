using CineClubApi.Common.DTOs.Movies;
using CineClubApi.Common.Enums;
using CineClubApi.Models;

namespace CineClubApi.Services.TMDBLibService;

public interface ITMDBMovieService
{
    Task<List<MovieForListDto>> GetMoviesByKeyword(string keyword);
    Task<DetailedMovieDto> getMovieById(int id);
    // Task<byte[]> GetMovieImage(string url);
    

}