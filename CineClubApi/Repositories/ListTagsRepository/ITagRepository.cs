using CineClubApi.Models;

namespace CineClubApi.Repositories.ListTagsRepository;

public interface ITagRepository
{
    Task<List<Tag>> GetAllTags();

    Task<bool> TagExistsByName(string name);
    Task AddTag(Tag tag);
    Task DeleteTag(Tag tag);
    Task<Tag> GetTagById(Guid tagId);
}