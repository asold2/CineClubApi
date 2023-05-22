using CineClubApi.Common.Interfaces;
using CineClubApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CineClubApi.Repositories.MovieRepository;

public class MovieRepositoryImpl : IMovieRepository
{

    private readonly IApplicationDbContext _applicationDbContext;

    public MovieRepositoryImpl(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }
    
    
    public async Task AddMovieToList(Guid listId, Guid movieDaoId)
    {
        var neededList = await _applicationDbContext.Lists.FirstOrDefaultAsync(x => x.Id == listId);

        var neededMovieDao = await _applicationDbContext.MovieDaos.FirstOrDefaultAsync(x => x.Id == movieDaoId);
        
        neededList.MovieDaos.Add(neededMovieDao);

        _applicationDbContext.Lists.Update(neededList);
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task<Guid> SaveMovieDao(MovieDao movieDao)
    {

        var neededMovieDao = 
            await _applicationDbContext.MovieDaos.FirstOrDefaultAsync(x => x.tmdbId == movieDao.tmdbId);

        if (neededMovieDao == null)
        {
            var savedMovieDao = await _applicationDbContext.MovieDaos.AddAsync(movieDao);
            await _applicationDbContext.SaveChangesAsync();
            return savedMovieDao.Entity.Id;
        }
        
        return neededMovieDao.Id;

    }

    public async Task<MovieDao> GetMovieByTmdbId(int tmdnId)
    {
        var result = await _applicationDbContext.MovieDaos.FirstOrDefaultAsync(x => x.tmdbId == tmdnId);
        return result;
    }
}