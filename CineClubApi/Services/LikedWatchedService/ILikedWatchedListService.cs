using CineClubApi.Common.DTOs.List;

namespace CineClubApi.Services.LikedWatchedService;

public interface ILikedWatchedListService
{
        Task<UpdateListDto> GetUsersLikedList(Guid userId);
        Task<UpdateListDto> GetUsersWatchedList(Guid userId);
}