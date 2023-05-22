using AutoMapper;
using AutoMapper.QueryableExtensions;
using CineClubApi.Common.DTOs.List;
using CineClubApi.Common.Interfaces;
using CineClubApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CineClubApi.Repositories.ListRepository;

public class ListRepositoryImpl : IListRepository
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public ListRepositoryImpl(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }



    public async Task CreateList(List list)
    {
        await _applicationDbContext.Lists.AddAsync(list);
        await _applicationDbContext.SaveChangesAsync();
    }



    public async Task<List> GetListById(Guid id)
    {
        return await _applicationDbContext.Lists.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task DeleteListById(Guid id)
    {
        var listToRemove = await _applicationDbContext.Lists.FirstOrDefaultAsync(x => x.Id == id);

        if (listToRemove != null)
        {
            _applicationDbContext.Lists.Remove(listToRemove);
            await _applicationDbContext.SaveChangesAsync();
        }

        else throw new Exception("Entity not found");

    }

    public async Task<bool> UserHasRightToUpdateList(Guid listId, Guid userId)
    {
        var neededList = await _applicationDbContext.Lists.FirstOrDefaultAsync(x=>x.Id==listId);

        if (neededList == null)
        {
            return false;
        }

        if (neededList.CreatorId == userId)
        {
            return true;
        }

        return false;
    }

    public async Task AddTagToList(Guid listId, Guid newTagId)
    {
        var neededList = await _applicationDbContext.Lists.FirstOrDefaultAsync(x => x.Id == listId);
        
        var neededTag =  await _applicationDbContext.ListTags.FirstOrDefaultAsync(x => x.Id == newTagId);
        
        neededList.Tags.Add(neededTag);

        await UpdateList(neededList);
    }

    public async Task UpdateList(List list)
    {
        _applicationDbContext.Lists.Update(list);
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task<List<UpdateListDto>> GetAllListsByUserId(Guid userId)
    {
        var lists = await _applicationDbContext.Lists
            .Where(x => x.CreatorId == userId)
            .ProjectTo<UpdateListDto>(_mapper.ConfigurationProvider).ToListAsync();
        return lists;
    }
}