using CineClubApi.Common.DTOs.List;
using CineClubApi.Common.ServiceResults;
using CineClubApi.Models;
using CineClubApi.Models.Auth;

namespace CineClubApi.Services.ListService;

public interface IListService
{
    public Task<ServiceResult> CreateNamedList(ListDto listDto);

    public Task<ServiceResult> UpdateListNameOrStatus(UpdateListDto updateListDto);

    public Task<IList<UpdateListDto>> GetListsByUserId(string tokenBody);

    public Task<ServiceResult> DeleteListById(Guid id);

}