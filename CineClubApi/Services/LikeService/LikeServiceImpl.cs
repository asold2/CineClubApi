using AutoMapper;
using CineClubApi.Common.DTOs.List;
using CineClubApi.Common.ServiceResults;
using CineClubApi.Models;
using CineClubApi.Repositories.AccountRepository;
using CineClubApi.Repositories.LikeRepository;
using CineClubApi.Repositories.ListRepository;

namespace CineClubApi.Services.LikeService;

public class LikeServiceImpl : ILikeService
{
    private readonly ILikeRepository _likeRepository;
    private readonly IListRepository _listRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public LikeServiceImpl(ILikeRepository likeRepository,
        IListRepository listRepository,
        IUserRepository userRepository,
        IMapper mapper)
    {
        _likeRepository = likeRepository;
        _listRepository = listRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }
    
    
    
    public async Task<ServiceResult> UserLikeList(Guid userId, Guid listId)
    {
        var neededUser = await _userRepository.GetUserById(userId);
        var neededList = await _listRepository.GetListById(listId);

        if (neededUser == null)
        {
            return new ServiceResult
            {
                Result = "User not found!",
                StatusCode = 400
            };
        }
        
        if (neededList == null)
        {
            return new ServiceResult
            {
                Result = "List not found!",
                StatusCode = 400
            };
        }

        if (!neededList.Public && !await _listRepository.UserHasRightToUpdateList(listId, userId))
        {
            return new ServiceResult
            {
                Result = "User cannot like a private list!",
                StatusCode = 403
            };
        }

        if (await _likeRepository.UserHasLikedList(userId, listId))
        {
            return new ServiceResult
            {
                Result = "User has already liked this list!",
                StatusCode = 409
            };
        }

        var like = new Like
        {
            UserId = userId,
            User = neededUser,
            ListId = listId,
            List = neededList
        };

        await _likeRepository.UserLikeList(like);
        return new ServiceResult
        {
            Result = "User liked the list.",
            StatusCode = 200
        };
    }

    public async Task<ServiceResult> UserUnlikeList(Guid userId, Guid listId)
    {
        var neededUser = await _userRepository.GetUserById(userId);
        var neededList = await _listRepository.GetListById(listId);

        if (neededUser == null)
        {
            return new ServiceResult
            {
                Result = "User not found!",
                StatusCode = 400
            };
        }
        
        if (neededList == null)
        {
            return new ServiceResult
            {
                Result = "List not found!",
                StatusCode = 400
            };
        }

        var neededLike = await _likeRepository.GetLikeByUserAndList(userId, listId);

        if (neededLike == null)
        {
            return new ServiceResult
            {
                Result = "User doesn't like this list anyway.",
                StatusCode = 409
            };
        }

        await _likeRepository.UserUnlikeList(neededLike);

        return new ServiceResult
        {
            Result = "User unliked list.",
            StatusCode = 200
        };



    }

    public async Task<List<UpdateListDto>> GetLikedLists(Guid userId)
    {
        var neededUser = await _userRepository.GetUserById(userId);

        if (neededUser==null)
        {
            return null;
        }

        var usersLikes = await _likeRepository.GetAllLikesByUserId(userId);

        var likedLists = new List<UpdateListDto>();

        foreach (var like in usersLikes)
        {

            if (! await _listRepository.UserHasRightToUpdateList(like.ListId, userId) && !like.List.Public )
            {
                continue;
            }
            
            
            var updatedListDto = _mapper.Map<UpdateListDto>(like.List);



            likedLists.Add(updatedListDto);
        }

        return likedLists;

    }
}