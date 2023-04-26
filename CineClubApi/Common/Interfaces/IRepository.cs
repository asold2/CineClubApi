using CineClubApi.Models;

namespace CineClubApi.Common.Interfaces;

public interface IRepository
{
    public Task CreateEntity(Entity entity);
    public Task<IList<Entity>> GetAllEntities(Entity entity);
    public Task<Entity> GetEntityById(Guid id);
    public Task DeleteEntityById(Guid id);
    public Task UpdateEntity(Entity entity);

}