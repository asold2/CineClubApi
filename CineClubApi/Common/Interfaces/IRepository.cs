using CineClubApi.Models;

namespace CineClubApi.Common.Interfaces;

public interface IRepository
{
    public Task CreateEntity(Entity entity);
    public Task<List<Entity>> GetAllEntities();
    public Task<Entity> GetEntityById(Guid id);
    public Task DeleteEntityById(Guid id);
    public Task UpdateEntity(Entity entity);

}