using CineClubApi.Common.DTOs.Genre;
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
    public DbSet<Tag> ListTags { get; }

    public DbSet<Like> Likes { get; }

    public DbSet<RatingGenreDto> GenreRatings { get; }


    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

}