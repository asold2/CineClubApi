using CineClubApi.Common.DTOs.Actor;
using CineClubApi.Common.DTOs.List;
using CineClubApi.Common.DTOs.Movies;
using CineClubApi.Common.Helpers;


namespace CineClubApi.Services.ListService;

public interface IListService
{
    

    public Task<IList<UpdateListDto>> GetListsByUserId(Guid userId);

    Task<List<ListDto>> GetListsByTags(List<Guid> tagIds);
    Task<UpdateListDto> AssignImageToList(UpdateListDto listDto);

    Task<DetailedListDto> GetListsById(Guid listId);
    Task<List<MoviePersonDto>> GetTop5ActorsByListId(Guid listId);

    Task<List<MoviePersonDto>> GetTop5DirectorsByListId(Guid listId);

    Task<PaginatedResult<UpdateListDto>> GetAllLists(int page, int start, int end);

}