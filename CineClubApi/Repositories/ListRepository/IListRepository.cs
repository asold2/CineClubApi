using CineClubApi.Common.DTOs.List;
using CineClubApi.Common.Interfaces;
using CineClubApi.Models;

namespace CineClubApi.Repositories.ListRepository;

public interface IListRepository
{
    public Task<List<UpdateListDto>> GetAllListsByUserId(Guid userId);


    Task CreateList(List list);

    Task<List> GetListById(Guid id);
    Task UpdateList(List list);

    Task DeleteListById(Guid id);
    Task<bool> UserHasRightToUpdateList(Guid listId, Guid userId);

    Task AddTagToList(Guid listId, Guid newTagId);

    Task<List> GetListWithMovies(Guid listId);

    Task<List<List>> GetAllPublicLists();

}