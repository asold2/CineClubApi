using CineClubApi.Common.DTOs.List;
using CineClubApi.Common.Interfaces;
using CineClubApi.Models;

namespace CineClubApi.Repositories.ListRepository;

public interface IListRepository : IRepository
{
    public Task<IList<UpdateListDto>> GetAllListsByUserId(Guid userId);
    Task<List<List>> GetAllLists();


}