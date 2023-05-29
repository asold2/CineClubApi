using CineClubApi.Common.DTOs.List;
using CineClubApi.Common.ServiceResults;

namespace CineClubApi.Services.ListService;

public interface IServiceResultListService
{
    public Task<ServiceResult> CreateNamedList(ListDto listDto);

    public Task<ServiceResult> UpdateListNameOrStatus(SimpleUpdateListDto updateListDto);
    public Task<ServiceResult> DeleteListById(Guid listId, Guid userId);

}