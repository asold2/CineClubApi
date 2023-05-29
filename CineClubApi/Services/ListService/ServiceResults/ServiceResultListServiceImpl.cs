using AutoMapper;
using CineClubApi.Common.DTOs.List;
using CineClubApi.Common.Helpers;
using CineClubApi.Common.ServiceResults;
using CineClubApi.Common.ServiceResults.ListResults;
using CineClubApi.Models;
using CineClubApi.Repositories.AccountRepository;
using CineClubApi.Repositories.ListRepository;
using CineClubApi.Repositories.ListTagsRepository;
using CineClubApi.Services.TmdbGenre;
using CineClubApi.Services.TMDBLibService;
using CineClubApi.Services.TMDBLibService.Actor;

namespace CineClubApi.Services.ListService;

public class ServiceResultListServiceImpl : ListService, IServiceResultListService
{
    
    public ServiceResultListServiceImpl(IListRepository listRepository, IUserRepository userRepository, IMapper mapper, ITagRepository tagRepository, IPaginator paginator, ITMDBMovieService movieService, ITMDBPeopleService peopleService, ITMDBGenreService genreService) : base(listRepository, userRepository, mapper, tagRepository, paginator, movieService, peopleService, genreService)
    {
    }
    
    
    public async Task<ServiceResult> DeleteListById(Guid listId, Guid userId)
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
            Creator = neededUser,
            CreatorId = neededUser.Id
        };


        await _listRepository.CreateList(list);
        return new CreatedListResult
        {
            Result = "Created new List named: " + list.Name,
            StatusCode = 200
        };
    }

    public async Task<ServiceResult> UpdateListNameOrStatus(SimpleUpdateListDto updateListDto)
    {
        var listToUpdate = await _listRepository.GetListById(updateListDto.Id);

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

}