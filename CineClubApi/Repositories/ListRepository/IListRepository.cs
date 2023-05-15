using CineClubApi.Common.DTOs.List;
using CineClubApi.Common.Interfaces;
using CineClubApi.Models;

namespace CineClubApi.Repositories.ListRepository;

public interface IListRepository
{
    public Task<IList<UpdateListDto>> GetAllListsByUserId(Guid userId);


    Task CreateList(List list);

    Task<List> GetListById(Guid id);
    Task UpdateList(List list);

    Task DeleteListById(Guid id);


}