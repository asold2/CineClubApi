using CineClubApi.Common.Interfaces;
using CineClubApi.Models;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;

namespace CineClubApi.Repositories.ListTagsRepository;

public class TagRepositoryImpl : ITagRepository
{
    private readonly IApplicationDbContext _applicationDbContext;

    public TagRepositoryImpl(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<List<Tag>> GetAllTags()
    {
        return await _applicationDbContext.ListTags.ToListAsync();
    }

    public async Task<bool> TagExistsByName(string name)
    {
        var result = await _applicationDbContext.ListTags.AnyAsync(x => x.Name.ToLower() == name.ToLower());
        return result;
    }

    public async Task AddTag(Tag tag)
    {
        await _applicationDbContext.ListTags.AddAsync(tag);
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task DeleteTag(Tag tag)
    {
        _applicationDbContext.ListTags.Remove(tag);
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task<Tag> GetTagById(Guid tagId)
    {
        var result = await _applicationDbContext.ListTags.FirstOrDefaultAsync(x => x.Id == tagId);
        return result;
    }
}