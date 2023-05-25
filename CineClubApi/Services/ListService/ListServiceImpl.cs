using AutoMapper;
using CineClubApi.Common.DTOs.List;
using CineClubApi.Common.Helpers;
using CineClubApi.Common.ServiceResults;
using CineClubApi.Common.ServiceResults.ListResults;
using CineClubApi.Models;
using CineClubApi.Models.Auth;
using CineClubApi.Repositories.AccountRepository;
using CineClubApi.Repositories.ListRepository;
using CineClubApi.Repositories.ListTagsRepository;
using CineClubApi.Services.ListTagService;
using Microsoft.EntityFrameworkCore;

namespace CineClubApi.Services.ListService;

public class ListServiceImpl : IListService
{

    private readonly IListRepository _listRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IMapper _mapper;
    private readonly IPaginator _paginator;
    
    public ListServiceImpl(IListRepository listRepository, IUserRepository userRepository, IMapper mapper, ITagRepository tagRepository, IPaginator paginator)
    {
        _listRepository = listRepository;
        _userRepository = userRepository;
        _mapper = mapper;
        _tagRepository = tagRepository;
        _paginator = paginator;
    }
    
    
    public async Task<ServiceResult> CreateNamedList(ListDto listDto)
    {

        var neededUser = await _userRepository.GetUserById(listDto.CreatorId);
        var usersLists = await _listRepository.GetAllListsByUserId(neededUser.Id);

        if (usersLists.Any(x => x.Name == listDto.Name))
        {
            return new ServiceResult
            {
                StatusCode = 409,
                Result = "User already has a list named like this one!"
            };

        }

        var list = new List
        {
            Name = listDto.Name,
            Public = listDto.Public,
            Creator  = neededUser,
            CreatorId = neededUser.Id
        };
        
        
        await _listRepository.CreateList(list);
        return new CreatedListResult
        {
            Result = "Created new List named: " + list.Name,
            StatusCode = 200
        };
    }

    public async Task<ServiceResult> UpdateListNameOrStatus(UpdateListDto updateListDto)
    {
        var listToUpdate =(List) await _listRepository.GetListById(updateListDto.Id);

        if (!await _listRepository.UserHasRightToUpdateList(updateListDto.Id, updateListDto.CreatorId))
        {
            return new ServiceResult
            {
                StatusCode = 403,
                Result = "User has no right to update this list!"
            };
        }


        if (listToUpdate == null)
        {
            return new EntityNotFoundResult();
        }

        listToUpdate.Name = updateListDto.Name;
        listToUpdate.Public = updateListDto.Public;
        
        await _listRepository.UpdateList(listToUpdate);

        return new ListSuccessfullyUpdateResult();
    }

    public async Task<IList<UpdateListDto>> GetListsByUserId(Guid userId)
    {
        var neededUser = await _userRepository.GetUserById(userId);

        if (neededUser==null)
        {
            return null;
        }
        
        
        var lists = await _listRepository.GetAllListsByUserId(neededUser.Id);

        var result = _mapper.ProjectTo<UpdateListDto>(lists.AsQueryable()).ToList();

        return result;


    }

    public  async Task<ServiceResult> DeleteListById(Guid listId, Guid userId)
    {

        if (!await _listRepository.UserHasRightToUpdateList(listId, userId))
        {
            return new ServiceResult
            {
                StatusCode = 403,
                Result = "User has no right to delete this list!"
            };
        }
        
        try
        {
            await _listRepository.DeleteListById(listId);
            return new ListDeletedResult();
        }
        catch (Exception e)
        {
            return new EntityNotFoundResult();
        }
    }

    public async Task<List<ListDto>> GetListsByTags(List<Guid> tagIds)
    {
        var listOfNeededTags = new List<Tag>();

        foreach (var id in tagIds)
        {
            var tag = await _tagRepository.GetTagWithListsById(id);
            if (tag == null)
            {
                continue;
            }
            listOfNeededTags.Add(tag);
        }

        var listOfNeededLists = new List<List>();

        foreach (var tag in listOfNeededTags)
        {
            listOfNeededLists.AddRange(tag.Lists);
        }

        var result =  _mapper.ProjectTo<ListDto>(listOfNeededLists.AsQueryable()).ToList();

        //getting only public lists
        result = result.Where(x => x.Public).ToList();

        return result;
    }

    public async Task<List<UpdateListDto>> GetAllLists(int page, int start, int end)
    {
        var lists = await _listRepository.GetAllPublicLists();


        var result = _mapper.ProjectTo<UpdateListDto>(lists.AsQueryable()).ToList();
        
        var paginatedResult =await  _paginator.PaginateUpdatedListDto(result, page, start, end);


        return paginatedResult;

    }

    public async Task<UpdateListDto> GetUsersLikedList(Guid userId)
    {
        var allLists = await _listRepository.GetAllListsByUserId(userId);

        var neededUser = await _userRepository.GetUserById(userId);

        if (neededUser == null)
        {
            return null;
        }
        
        var likedList = allLists.FirstOrDefault(x => x.Name == "Liked Movies");
        
        if (likedList==null)
        {
            var newLikedList = new List
            {
                Name = "Liked Movies",
                Public = false,
                CreatorId = userId,
                Creator = neededUser
            };
            await _listRepository.CreateList(newLikedList);

            return _mapper.Map<UpdateListDto>(newLikedList);
        }

        var result = _mapper.Map<UpdateListDto>(likedList);
        
        return result;

    }
    
    public async Task<UpdateListDto> GetUsersWatchedList(Guid userId)
    {
        var allLists = await _listRepository.GetAllListsByUserId(userId);

        var neededUser = await _userRepository.GetUserById(userId);

        if (neededUser == null)
        {
            return null;
        }

        var watchedList = allLists.FirstOrDefault(x => x.Name == "Watched Movies");

        if (watchedList==null)
        {
            var newWatchedList = new List
            {
                Name = "Watched Movies",
                Public = false,
                CreatorId = userId,
                Creator = neededUser
            };
            await _listRepository.CreateList(newWatchedList);

            return _mapper.Map<UpdateListDto>(newWatchedList);

        }

        var result = _mapper.Map<UpdateListDto>(watchedList);
        
        return result;

    }
}