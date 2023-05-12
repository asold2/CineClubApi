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
        
            var connectionString = Environment.GetEnvironmentVariable("AzureCineClubDb");
            optionsBuilder.UseNpgsql("Server=cineclubdb.postgres.database.azure.com;Database=cineclubdb;Port=5432;User Id=postgres@cineclubdb;Password=Saphira123;");

    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        modelBuilder.Entity<User>().ToTable("Users");
    }


    // public DbSet<Entity> Entities { get; }
    public DbSet<User> Users => Set<User>();
    public DbSet<List> Lists => Set<List>();
}