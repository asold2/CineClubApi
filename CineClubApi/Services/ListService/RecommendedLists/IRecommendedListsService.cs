using CineClubApi.Common.DTOs.List;
using CineClubApi.Common.DTOs.Movies;
using CineClubApi.Common.Helpers;

namespace CineClubApi.Services.ListService.RecommendedLists;

public interface IRecommendedListsService
{
    Task<DetailedListDto> GetListOfRecommendedMoviesForUser(Guid userId, int page, int start,
        int end);
}