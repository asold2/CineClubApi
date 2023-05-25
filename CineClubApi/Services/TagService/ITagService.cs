using CineClubApi.Common.DTOs.Tag;
using CineClubApi.Common.ServiceResults;
using CineClubApi.Common.ServiceResults.TagResults;
using CineClubApi.Models;

namespace CineClubApi.Services.ListTagService;

public interface ITagService
{
    Task<List<TagForListDto>> GetAllTags();
    Task<CreatedTagResult> CreateTag( Guid userId, string name);
    Task<ServiceResult> AssignTagToList(Guid tagId, Guid listId, Guid userId);
    Task<ServiceResult> DeleteTag(Guid tagId, Guid userId);
    Task<TagDto> GetTag(Guid tagId);
}