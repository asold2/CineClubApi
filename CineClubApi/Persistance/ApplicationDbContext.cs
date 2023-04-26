using CineClubApi.Common.Interfaces;
using CineClubApi.Models;
using CineClubApi.Models.Auth;
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
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("CineClubDb"));
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        modelBuilder.Entity<User>().ToTable("Users");
    }


    // public DbSet<Entity> Entities { get; }
    public DbSet<User> Users => Set<User>();
}