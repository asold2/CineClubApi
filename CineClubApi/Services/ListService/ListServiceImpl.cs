using AutoMapper;
using CineClubApi.Common.DTOs.Actor;
using CineClubApi.Common.DTOs.List;
using CineClubApi.Common.DTOs.Movies;
using CineClubApi.Common.DTOs.Tag;
using CineClubApi.Common.Helpers;
using CineClubApi.Common.ServiceResults;
using CineClubApi.Common.ServiceResults.ListResults;
using CineClubApi.Models;
using CineClubApi.Models.Auth;
using CineClubApi.Repositories.AccountRepository;
using CineClubApi.Repositories.ListRepository;
using CineClubApi.Repositories.ListTagsRepository;
using CineClubApi.Services.ListTagService;
using CineClubApi.Services.TMDBLibService;
using CineClubApi.Services.TMDBLibService.Actor;
using Microsoft.EntityFrameworkCore;

namespace CineClubApi.Services.ListService;

public class ListServiceImpl : IListService
{

    private readonly IListRepository _listRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IMapper _mapper;
    private readonly IPaginator _paginator;
    private ITMDBMovieService _movieService;
    private ITMDBPeopleService _peopleService; 
    
    public ListServiceImpl(IListRepository listRepository, 
        IUserRepository userRepository, 
        IMapper mapper, 
        ITagRepository tagRepository,
        IPaginator paginator,
        ITMDBMovieService movieService,
        ITMDBPeopleService peopleService)
    {
        _listRepository = listRepository;
        _userRepository = userRepository;
        _mapper = mapper;
        _tagRepository = tagRepository;
        _paginator = paginator;
        _movieService = movieService;
        _peopleService = peopleService;
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

    public async Task<IList<UpdateListDto>> GetListsByUserId(Guid userId)
    {
        var neededUser = await _userRepository.GetUserById(userId);

        if (neededUser==null)
        {
            return null;
        }
        
        
        var lists = await _listRepository.GetAllListsByUserId(neededUser.Id);

        lists = lists.Where(x => x.Name != "Liked Movies" && x.Name != "Watched Movies").ToList();

        var result = _mapper.ProjectTo<UpdateListDto>(lists.AsQueryable()).ToList();

        foreach (var tempList in result)
        {
            var l = await AssignImageToList(tempList);
            tempList.BackdropPath = l.BackdropPath;

        }

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

        foreach (var tempList in paginatedResult)
        {
            var l = await AssignImageToList(tempList);
            tempList.BackdropPath = l.BackdropPath;

        }
        
        
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

        result = await AssignImageToList(result);
        
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
        
        result = await AssignImageToList(result);

        
        return result;

    }

    public async Task<UpdateListDto> AssignImageToList(UpdateListDto listDto)
    {
        var neededList = await _listRepository.GetListWithMovies(listDto.Id);


        if (neededList.MovieDaos.Count==0)
        {
            return listDto;
        }

        var randomMovie = neededList.MovieDaos.FirstOrDefault();

        var movieFromTmdb = await _movieService.getMovieById(randomMovie.tmdbId);

        listDto.BackdropPath = movieFromTmdb.BackdropPath;
        listDto.MovieName = movieFromTmdb.OriginalTitle;
        return listDto;


    }

    public async Task<DetailedListDto> GetListsById(Guid listId)
    {
        var neededList = await _listRepository.GetListByIdWithEverythingIncluded(listId);

        if (neededList==null)
        {
            return new DetailedListDto();
        }

        // var movieDtos = _mapper.ProjectTo<MovieForListDto>(neededList.MovieDaos.AsQueryable()).ToList();
        var tagDtos = _mapper.ProjectTo<TagForListDto>(neededList.Tags.AsQueryable()).ToList();
        
        
        var result = _mapper.Map<DetailedListDto>(neededList);

        // result.MovieDtos = movieDtos;
        result.TagsDtos = tagDtos;

        var topActorsFromEachMove = new List<MoviePersonDto>();
        
        foreach (var movie in neededList.MovieDaos)
        {

            var tmdbMovie = await _movieService.getMovieById(movie.tmdbId);
            var movieDto = _mapper.Map<MovieForListDto>(tmdbMovie);
            
            result.MovieDtos.Add(movieDto);
            
            var top15Actors = await _peopleService.GetAllActors(movie.tmdbId);

            topActorsFromEachMove.AddRange(top15Actors);
        }
        
        var top5Actors = topActorsFromEachMove
            .OrderByDescending(actor => actor.Popularity)
            .Take(5)
            .ToList();

        result.Top5ActorsFromList = top5Actors;
        

        return result;
    }
}