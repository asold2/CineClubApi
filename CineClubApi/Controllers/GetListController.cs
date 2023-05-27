using CineClubApi.Common.DTOs.Actor;
using CineClubApi.Common.DTOs.List;
using CineClubApi.Common.DTOs.Movies;
using CineClubApi.Common.Helpers;
using CineClubApi.Common.Permissions;
using CineClubApi.Models;
using CineClubApi.Services.ListService;
using CineClubApi.Services.ListService.LikedList;
using CineClubApi.Services.ListService.WatchedList;
using CineClubApi.Services.MovieService;
using Microsoft.AspNetCore.Mvc;

namespace CineClubApi.Controllers;

public class GetListController : CineClubControllerBase
{ 
    
    private readonly IListService _listService;
    private readonly ILikedListService _likedListService;
    private readonly IWatchedListService _watchedListService;

    public GetListController(IListService listService, IWatchedListService watchedListService, ILikedListService likedListService)
    {
        _watchedListService = watchedListService;
        _likedListService = likedListService;
        _listService = listService;
    }
    
    [HttpGet("lists")]
    public async Task<ActionResult<IList<UpdateListDto>>> GetListsByUserdId([FromQuery] Guid userId)
    {
        var result = await _listService.GetListsByUserId(userId);

        if (result==null)
        {
            return new ContentResult
            {
                Content = "User not found",
                ContentType = "text/plain",
                StatusCode = 400
            };
        }
        
        return Ok(result);
    }
    
    [HttpGet("all_lists")]
    public async Task<ActionResult<PaginatedResult<UpdateListDto>>> GetAllLists([FromQuery]int page, [FromQuery]int start,[FromQuery] int end)
    {
        var result = await _listService.GetAllLists(page, start, end);

        return Ok(result);

    }

    [LoggedInPermission]
    [HttpGet("liked_list")]
    public async Task<ActionResult<UpdateListDto>> GetUsersLikedList([FromQuery] Guid userId)
    {
        var result = await _likedListService.GetUsersLikedList(userId);

        return Ok(result);
    }
    [LoggedInPermission]
    [HttpGet("watched_list")]
    public async Task<ActionResult<UpdateListDto>> GetUsersWatchedList([FromQuery] Guid userId)
    {
        var result = await _watchedListService.GetUsersWatchedList(userId);

        return Ok(result);
    }
    
    
    
    [HttpGet("lists/tags")]
    public async Task<List<ListDto>> GetListsWithTags([FromQuery] List<Guid> tagIds)
    {
        var result = await _listService.GetListsByTags(tagIds);

        return result;

    }

    [HttpGet("list/")]
    public async Task<ActionResult<List>> GetListById([FromQuery] Guid listId)
    {

        // var neededGuid = Guid.NewGuid(listId);
        
        var result = await _listService.GetListsById(listId);

        return Ok(result);
    }

    [HttpGet("list/top_actors")]
    public async Task<List<MoviePersonDto>> GetTop5ActorsPerList([FromQuery] Guid listId)
    {
        return await _listService.GetTop5ActorsByListId(listId);
    }
    
    [HttpGet("list/top_directors")]
    public async Task<List<MoviePersonDto>> GetTop5DirectorsPerList([FromQuery] Guid listId)
    {
        return await _listService.GetTop5DirectorsByListId(listId);
    }


    [HttpGet("moive/liked_watched")]
    public async Task<ActionResult<MovieLikedWathedDto>> GetMovieLikedWatchedByUser([FromQuery] Guid userId, [FromQuery]int tmdbId)
    {
        var watched = await _watchedListService.MovieBelongsToWatched(userId, tmdbId);
        var liked = await _likedListService.MovieBelongsToLiked(userId, tmdbId);

        return Ok(new MovieLikedWathedDto { Watched = watched, Liked = liked});
    }

}