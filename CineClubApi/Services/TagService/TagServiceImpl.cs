﻿using AutoMapper;
using CineClubApi.Common.DTOs.Tag;
using CineClubApi.Common.ServiceResults;
using CineClubApi.Common.ServiceResults.TagResults;
using CineClubApi.Models;
using CineClubApi.Repositories.AccountRepository;
using CineClubApi.Repositories.ListRepository;
using CineClubApi.Repositories.ListTagsRepository;
using CineClubApi.Services.ListService;
using CineClubApi.Services.ListService.LikedList;
using CineClubApi.Services.ListService.WatchedList;
using Microsoft.EntityFrameworkCore;

namespace CineClubApi.Services.ListTagService;

public class TagServiceImpl : ITagService
{
    private readonly ITagRepository _tagRepository;
    private readonly IUserRepository _userRepository;
    private readonly IListRepository _listRepository;
    private readonly IMapper _mapper;
    private readonly ILikedListService _likedListService;
    private readonly IWatchedListService _watchedListService;
    private IListService _listService;

    public TagServiceImpl(ITagRepository tagRepository,
        IUserRepository userRepository,
        IListRepository listRepository,
        IMapper mapper,
        ILikedListService likedListService,
        IWatchedListService watchedListService,
        IListService listService
        )
    {
        _tagRepository = tagRepository;
        _userRepository = userRepository;
        _listRepository = listRepository;
        _mapper = mapper;
        _likedListService = likedListService;
        _watchedListService = watchedListService;
        _listService = listService;
    }
    
    
    public async Task<List<TagForListDto>> GetAllTags()
    {
        var result = await _tagRepository.GetAllTags();

        var neededList = await _mapper.ProjectTo<TagForListDto>(result).ToListAsync();
        
        return neededList;
    }

    public async Task<CreatedTagResult> CreateTag( Guid userId, string name)
    {
        if (await _tagRepository.TagExistsByName(name))
        {
            return new CreatedTagResult
            {
                StatusCode = 409,
                Result = "Tag with this name already exists!"
            };
        }


        var creator = await _userRepository.GetUserById(userId);

        if (creator==null)
        {
            return new CreatedTagResult
            {
                StatusCode = 400,
                Result = "User not found!"
            };
        }

        var newTag = new Tag
        {
            Name = name.ToLower(),
            CreatorId = userId,
            Creator = creator
        };
    
        await _tagRepository.AddTag(newTag);
        
        return new CreatedTagResult
        {
            StatusCode = 200,
            Result = "Tag created!",
            tagId = newTag.Id
        };
    }

    public async  Task<ServiceResult> AssignTagToList(Guid tagId, Guid listId, Guid userId)
    {

        var neededTag = await _tagRepository.GetTagById(tagId);

        if (neededTag==null)
        {
            return new ServiceResult
            {
                StatusCode = 400,
                Result = "Tag not found!"
            };
        }
        
        
        var creator = await _userRepository.GetUserById(userId);

        if (creator==null)
        {
            return new ServiceResult
            {
                StatusCode = 400,
                Result = "User not found!"
            };
        }

        var listBelongsToUser = await _listRepository.UserHasRightToUpdateList(listId, userId);
        
        if (!listBelongsToUser)
        {
            return new ServiceResult
            {
                StatusCode = 403,
                Result = "User has no right to update this list or the list wasn't found!"
            };
        }
        

        var neededList = await _listRepository.GetListById(listId);

        if (neededList == null)
        {
            return new ServiceResult
            {
                StatusCode = 400,
                Result = "List not found!"
            };
        }

        if (neededTag.Lists.Any(x => x.Id == listId))
        {
            return new ServiceResult
            {
                StatusCode = 409,
                Result = "Tag already assigned to list!"
            }; 
        }
        

        await _listRepository.AddTagToList(listId, tagId);

        return new ServiceResult
        {
            StatusCode = 200,
            Result = "Tag assigned to list!"
        };
        
    }

    public async Task<ServiceResult> DeleteTag(Guid tagId, Guid userId)
    {
        var creator = await _userRepository.GetUserById(userId);

        if (creator==null)
        {
            return new ServiceResult
            {
                StatusCode = 400,
                Result = "User not found!"
            };
        }

        if ( ! await _tagRepository.UserHasRightToModifyTag(tagId, userId))
        {
            return new ServiceResult
            {
                StatusCode = 403,
                Result = "User cannot delete tag"
            };
        }

        var neededTag = await _tagRepository.GetTagById(tagId);

        if (neededTag == null)
        {
            return new ServiceResult
            {
                StatusCode = 400,
                Result = "Tag not found!"
            };
        }

        await _tagRepository.DeleteTag(neededTag);

        return new ServiceResult
        {
            StatusCode = 200,
            Result = "Tag deleted!"
        };

    }

    public async Task<TagDto> GetTag(Guid tagId)
    {
        var neededTag = await _tagRepository.GetTagById(tagId);

        if (neededTag == null)
        {
            return null;
        }

        var result = _mapper.Map<TagDto>(neededTag);

        return result;
    }

    public async Task<List<TagDto>> GetTagsForUsersLists(Guid userId)
    {
        var watchedlist = await _watchedListService.GetUsersWatchedList(userId);

        var likedList = await _likedListService.GetUsersLikedList(userId);

        var usersLists = await _listService.GetListsByUserId(userId);

        var neededTags = new List<TagDto>();
        
        neededTags.AddRange(watchedlist.Tags);
        neededTags.AddRange(likedList.Tags);
        foreach (var list in usersLists)
        {
            neededTags.AddRange(list.Tags);
        }

        var uniqueTags = new List<TagDto>();

        foreach (var tag in neededTags)
        {
            if (!uniqueTags.Any(t => t.Id == tag.Id))
            {
                uniqueTags.Add(tag);
            }
        }

        return uniqueTags;
    }
}