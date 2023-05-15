using CineClubApi.Models;
using CineClubApi.Models.Auth;
using Microsoft.EntityFrameworkCore;

namespace CineClubApi.Common.Interfaces;

public interface IApplicationDbContext 
{
    // public DbSet<Entity> Entities { get; }
    public DbSet<User> Users { get; }

    public DbSet<List> Lists { get; }
    public DbSet<MovieDao> MovieDaos{ get;  }


    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

}