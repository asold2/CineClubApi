using CineClubApi.Common.ServiceResults;
using CineClubApi.Models;

namespace CineClubApi.Services.ListTagService;

public interface ITagService
{
    Task<List<Tag>> GetAllTags();
    Task<ServiceResult> AddTag(Guid listId, Guid userId, string name);
    Task DeleteTag(Guid tagId, Guid userId);
    Task<Tag> GetTag(Guid tagId);
}