﻿using CineClubApi.Common.DTOs.Genre;
using CineClubApi.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CineClubApi.Repositories.StatisticsRepository;

public class StatisticsRepoImpl : IStatisticsRepo
{

    private readonly IApplicationDbContext _applicationDbContext;

    public StatisticsRepoImpl(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }
    
    
    public async Task SaveGenreStatistic(RatingGenreDto genreDto)
    {
        await _applicationDbContext.GenreRatings.AddAsync(genreDto);
        await _applicationDbContext.SaveChangesAsync();

    }

    public async Task<List<RatingGenreDto>> GetGenreStatistics()
    {
        return await _applicationDbContext.GenreRatings
            .Include(x=>x.Genre)
            .ToListAsync();
    }

    public async Task<bool> GenreratingsNeedUpdated()
    {

        if (await _applicationDbContext.GenreRatings.CountAsync()==0)
        {
            return true;
        }
        
        var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);
        var result = await _applicationDbContext.GenreRatings
            .AnyAsync(x => x.CreatedAt <= thirtyDaysAgo);

        if (result)
        {
            var allGenreRatings = await _applicationDbContext.GenreRatings.ToListAsync();
            _applicationDbContext.GenreRatings.RemoveRange(allGenreRatings);
            await _applicationDbContext.SaveChangesAsync();
        }
        
        return result;
    }
}