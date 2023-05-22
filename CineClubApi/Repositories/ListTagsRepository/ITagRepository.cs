using CineClubApi.Models;

namespace CineClubApi.Repositories.ListTagsRepository;

public interface ITagRepository
{
    Task<IQueryable<Tag>> GetAllTags();

    Task<bool> TagExistsByName(string name);
    Task AddTag(Tag tag);
    Task DeleteTag(Tag tag);
    Task<Tag> GetTagById(Guid tagId);
    Task<bool> UserHasRightToModifyTag(Guid tagId, Guid userId);

    Task<Tag> GetTagWithListsById(Guid id);
}