using AutoMapper;
using CineClubApi.Common.DTOs.Tag;
using CineClubApi.Common.ServiceResults;
using CineClubApi.Models;
using CineClubApi.Repositories.AccountRepository;
using CineClubApi.Repositories.ListRepository;
using CineClubApi.Repositories.ListTagsRepository;
using Microsoft.EntityFrameworkCore;

namespace CineClubApi.Services.ListTagService;

public class TagServiceImpl : ITagService
{
    private readonly ITagRepository _tagRepository;
    private readonly IUserRepository _userRepository;
    private readonly IListRepository _listRepository;
    private readonly IMapper _mapper;

    public TagServiceImpl(ITagRepository tagRepository, IUserRepository userRepository, IListRepository listRepository, IMapper mapper)
    {
        _tagRepository = tagRepository;
        _userRepository = userRepository;
        _listRepository = listRepository;
        _mapper = mapper;
    }
    
    
    public async Task<List<TagForListDto>> GetAllTags()
    {
        var result = await _tagRepository.GetAllTags();

        var neededList = await _mapper.ProjectTo<TagForListDto>(result).ToListAsync();
        
        return neededList;
    }

    public async Task<ServiceResult> CreateTag( Guid userId, string name)
    {
        if (await _tagRepository.TagExistsByName(name))
        {
            return new ServiceResult
            {
                StatusCode = 409,
                Result = "Tag with this name already exists!"
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

        var newTag = new Tag
        {
            Name = name.ToLower(),
            CreatorId = userId,
            Creator = creator
        };
    
        await _tagRepository.AddTag(newTag);
        
        return new ServiceResult
        {
            StatusCode = 200,
            Result = "Tag created with id:" +
                     newTag.Id
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
}