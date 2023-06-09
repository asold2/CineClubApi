﻿using CineClubApi.Common.DTOs.Genre;
using CineClubApi.Common.DTOs.Statistics;
using CineClubApi.Common.Interfaces;
using CineClubApi.Models;
using CineClubApi.Models.Auth;
using CineClubApi.Models.Statistics;
using Microsoft.EntityFrameworkCore;

namespace CineClubApi.Persistance;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly IConfiguration _configuration;
 
    
    public ApplicationDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
        
    }
    
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = Environment.GetEnvironmentVariable("AzureCineClubDb");
            optionsBuilder.UseNpgsql(connectionString);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        modelBuilder.Entity<User>().ToTable("Users");
    }


    public DbSet<User> Users => Set<User>();
    public DbSet<List> Lists => Set<List>();
    public DbSet<MovieDao> MovieDaos => Set<MovieDao>();
    public DbSet<Tag> ListTags  => Set<Tag>();
    public DbSet<Like> Likes => Set<Like>();
    public DbSet<RatingGenreDto> GenreRatings => Set<RatingGenreDto>();
    public DbSet<NumberOfMoviesPerYear> MoviesPerYears => Set<NumberOfMoviesPerYear>();
    public DbSet<RatingPerDirector> RatingPerDirectors => Set<RatingPerDirector>();
}