using CineClubApi.Common.ServiceResults;
using CineClubApi.Models;
using CineClubApi.Repositories.AccountRepository;
using CineClubApi.Repositories.ListRepository;
using CineClubApi.Repositories.ListTagsRepository;

namespace CineClubApi.Services.ListTagService;

public class TagServiceImpl : ITagService
{
    private readonly ITagRepository _tagRepository;
    private readonly IUserRepository _userRepository;
    private readonly IListRepository _listRepository;

    public TagServiceImpl(ITagRepository tagRepository, IUserRepository userRepository, IListRepository listRepository)
    {
        _tagRepository = tagRepository;
        _userRepository = userRepository;
        _listRepository = listRepository;
    }
    
    
    public async Task<List<Tag>> GetAllTags()
    {
        var result = await _tagRepository.GetAllTags();
        return result;
    }

    public async Task<ServiceResult> AddTag(Guid listId, Guid userId, string name)
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
        
        
        var newTag = new Tag
        {
            Name = name.ToLower(),
            CreatorId = userId,
            Creator = creator
        };
        
        // newTag.Lists.Add(neededList);

        
        await _tagRepository.AddTag(newTag);
        
        await _listRepository.AddTagToList(listId, newTag.Id);

        
        return new ServiceResult
        {
            StatusCode = 200,
            Result = "Tag created!"
        };
    }

    public Task DeleteTag(Guid tagId, Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<Tag> GetTag(Guid tagId)
    {
        throw new NotImplementedException();
    }
}