using CineClubApi.Common.DTOs.List;

namespace CineClubApi.Services.ListService.WatchedList;

public interface IWatchedListService
{
    Task<UpdateListDto> GetUsersWatchedList(Guid userId);
    Task<bool> MovieBelongsToWatched(Guid userId, int tmdbId);

}