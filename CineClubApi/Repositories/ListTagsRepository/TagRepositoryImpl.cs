using CineClubApi.Common.Interfaces;
using CineClubApi.Models;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;

namespace CineClubApi.Repositories.ListTagsRepository;

public class TagRepositoryImpl : ITagRepository
{
    private readonly IApplicationDbContext _applicationDbContext;

    public TagRepositoryImpl(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<IQueryable<Tag>> GetAllTags()
    {
        return  _applicationDbContext.ListTags.AsSingleQuery();
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

    public async Task<bool> UserHasRightToModifyTag(Guid tagId, Guid userId)
    {
        var neededTag = await _applicationDbContext.ListTags.FirstOrDefaultAsync(x=>x.Id==tagId);

        if (neededTag == null)
        {
            return false;
        }

        if (neededTag.CreatorId != userId)
        {
            return false;
        }

        return true;


    }

    public async Task<Tag> GetTagWithListsById(Guid id)
    {
        var result = await _applicationDbContext.ListTags
            .Include(x=>x.Lists)
            .FirstOrDefaultAsync(x => x.Id == id);
        return result;
    }
}