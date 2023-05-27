using CineClubApi.Common.DTOs.Movies;
using CineClubApi.Common.Helpers;

namespace CineClubApi.Services.ListService.RecommendedLists;

public interface IRecommendedListsService
{
    Task<PaginatedResult<List<MovieForListDto>>> GetListOfRecommendedMoviesForUser(Guid userId, int page, int start,
        int end);
}