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
    
    
    public async Task CreateEntity(Entity entity)
    {
        await _applicationDbContext.Lists.AddAsync((List)entity);
        await _applicationDbContext.SaveChangesAsync();
    }

    
    
    public Task<IList<Entity>> GetAllEntities(Entity entity)
    {
        throw new NotImplementedException();
    }

    public async Task<Entity> GetEntityById(Guid id)
    {
        return await _applicationDbContext.Lists.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task DeleteEntityById(Guid id)
    {
        var listToRemove = await _applicationDbContext.Lists.FirstOrDefaultAsync(x => x.Id == id);

        if (listToRemove != null)
        {
            _applicationDbContext.Lists.Remove(listToRemove);
            await _applicationDbContext.SaveChangesAsync();
        }

        else throw new Exception("Entity not found");

    }

    public async Task UpdateEntity(Entity entity)
    {
        _applicationDbContext.Lists.Update((List) entity);
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task<IList<UpdateListDto>> GetAllListsByUserId(Guid userId)
    {
        var lists = await _applicationDbContext.Lists
            .Where(x => x.CreatorId == userId)
            .ProjectTo<UpdateListDto>(_mapper.ConfigurationProvider).ToListAsync();
        return lists;
    }
}