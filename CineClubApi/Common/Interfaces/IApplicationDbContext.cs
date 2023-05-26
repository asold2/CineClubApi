using CineClubApi.Common.DTOs.Genre;
using CineClubApi.Common.DTOs.Statistics;
using CineClubApi.Models;
using CineClubApi.Models.Auth;
using CineClubApi.Models.Statistics;
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
    public DbSet<NumberOfMoviesPerYear> MoviesPerYears { get; }
    public DbSet<RatingPerDirector> RatingPerDirectors { get; }


    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

}