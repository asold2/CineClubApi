using CineClubApi.Common.DTOs.Movies;
using CineClubApi.Models;

namespace CineClubApi.Services.TMDBLibService;

public interface ITMDBLibService
{
    Task<List<MovieForListDto>> GetMoviesByKeyword(string keyword);
    Task<DetailedMovieDto> getMovieById(int id);
    Task<byte[]> GetMovieImage(string url);
}