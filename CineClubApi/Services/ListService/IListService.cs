using CineClubApi.Common.DTOs.List;
using CineClubApi.Common.ServiceResults;
using CineClubApi.Models;
using CineClubApi.Models.Auth;

namespace CineClubApi.Services.ListService;

public interface IListService
{
    public Task<ServiceResult> CreateNamedList(ListDto listDto);

    public Task<ServiceResult> UpdateListNameOrStatus(UpdateListDto updateListDto);

    public Task<IList<UpdateListDto>> GetListsByUserId(Guid userId);

    public Task<ServiceResult> DeleteListById(Guid listId, Guid userId);


    Task<List<ListDto>> GetListsByTags(List<Guid> tagIds);
    Task<List<UpdateListDto>> GetAllLists(int page, int start, int end);
    Task<UpdateListDto> GetUsersLikedList(Guid userId);
    Task<UpdateListDto> GetUsersWatchedList(Guid userId);
}