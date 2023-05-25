using CineClubApi.Common.DTOs.List;
using CineClubApi.Common.ServiceResults;

namespace CineClubApi.Services.LikeService;

public interface ILikeService
{
    Task<ServiceResult> UserLikeList(Guid userId, Guid listId);
    Task<ServiceResult> UserUnlikeList(Guid userId, Guid listId);

    Task<List<UpdateListDto>> GetLikedLists(Guid userId);
}