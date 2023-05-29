using CineClubApi.Common.DTOs.List;

namespace CineClubApi.Services.MovieService.MovieListService;

public interface IMovieListService 
{
    Task<List<SimpleListDto>> GetUsersListsToWhichMovieBelongs(Guid userId, int tmdbId);
    Task<List<SimpleListDto>> GetUsersListsMovieDoesNotBelongTo(Guid userId, int tmdbId);

    Task<List<UpdateListDto>> GetAllListsMoviebelongsTo(int tmdbId);
    
}