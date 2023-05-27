using CineClubApi.Common.DTOs.List;

namespace CineClubApi.Services.ListService.LikedList;

public interface ILikedListService
{
    Task<UpdateListDto> GetUsersLikedList(Guid userId);
    Task<bool> MovieBelongsToLiked(Guid userId, int tmdbId);

}