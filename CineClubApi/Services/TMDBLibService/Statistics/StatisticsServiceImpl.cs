﻿using AutoMapper;
using CineClubApi.Common.DTOs.Genre;
using CineClubApi.Common.DTOs.Movies;
using CineClubApi.Common.DTOs.Statistics;
using CineClubApi.Common.Helpers;
using CineClubApi.Repositories.StatisticsRepository;
using CineClubApi.Services.TmdbGenre;
using TMDbLib.Objects.Discover;
using TMDbLib.Objects.General;

namespace CineClubApi.Services.TMDBLibService.Statistics;

public class StatisticsServiceImpl :TmdbLib, IStatisticsService
{

    private readonly ITMDBMovieService _tmdbMovieService;
    private readonly ITMDBGenreService _genreService;
    private readonly IStatisticsRepo _statisticsRepo;
    
    public StatisticsServiceImpl(IMapper mapper,
        IPaginator paginator,
        ITMDBMovieService tmdbMovieService,
        ITMDBGenreService genreService,
        IStatisticsRepo statisticsRepo) : base(mapper, paginator)
    {
        _tmdbMovieService = tmdbMovieService;
        _genreService = genreService;
        _statisticsRepo = statisticsRepo;
    }

    public async Task<List<MovieForListDto>> TopGrossingMoviesOfAllTime()
    {

        var topGrossingMovies = await client.DiscoverMoviesAsync()
            .OrderBy(DiscoverMovieSortBy.RevenueDesc)
            .Query();

        var movieDtos = _mapper.ProjectTo<MovieForListDto>(topGrossingMovies.Results.Take(10).AsQueryable()).ToList();

        foreach (var movieDto in movieDtos)
        {
            var detailedMovie = await _tmdbMovieService.getMovieById(movieDto.Id);

            movieDto.Budget = detailedMovie.Budget;
            movieDto.Revenue = detailedMovie.Revenue;

        }
        
        return movieDtos;
    }

    public async Task<List<RatingGenreDto>> GetAverageRatingByGenre()
    {

        if (await _statisticsRepo.GenreratingsNeedUpdated())
        {
            const int pageSize = 50; // Number of movies to retrieve per page
            const int totalMovies = 10000; // Total number of movies to retrieve

            var ratingGenreDtos = new List<RatingGenreDto>();

            int moviesProcessed = 0;
            int currentPage = 1;

            while (moviesProcessed < totalMovies)
            {
                var movieDtos = await client.DiscoverMoviesAsync()
                    .Query(currentPage);

                foreach (var movie in movieDtos.Results)
                {
                    foreach (var genreId in movie.GenreIds)
                    {
                        var ratingGenreDto = ratingGenreDtos.FirstOrDefault(r => r.Genre.Id == genreId);
                        if (ratingGenreDto == null)
                        {
                            ratingGenreDto = new RatingGenreDto
                            {
                                Genre = new Genre {Id = genreId},
                                AvgRating = movie.VoteAverage
                            };
                            ratingGenreDtos.Add(ratingGenreDto);
                        }
                        else
                        {
                            ratingGenreDto.AvgRating = (ratingGenreDto.AvgRating + movie.VoteAverage) / 2;
                        }
                    }

                    moviesProcessed++;
                    if (moviesProcessed >= totalMovies)
                        break;
                }

                currentPage++;
            }

            foreach (var ratingGenreDto in ratingGenreDtos)
            {
                var detailedGenre = await _genreService.GetGenreById(ratingGenreDto.Genre.Id);
                ratingGenreDto.Genre.Name = detailedGenre.Name;
                ratingGenreDto.Genre.Id = detailedGenre.Id;

                await _statisticsRepo.SaveGenreStatistic(ratingGenreDto);
            }

            return ratingGenreDtos;
        }
        else
        {
            return await _statisticsRepo.GetGenreStatistics();
        }
        
    }

    public async Task<List<NumberOfMoviesPerYearDto>> GetNumberOfMoviesPerYear()
    {
        var listMoviesPerYear = await _statisticsRepo.GetNumberOfMoviesPerYear();
        
        if (listMoviesPerYear.Count!=0)
        {
            var result = _mapper.ProjectTo<NumberOfMoviesPerYearDto>(listMoviesPerYear.AsQueryable()).ToList();
            return result;
        }
        
        
        
        // Get the current year
        int currentYear = DateTime.UtcNow.Year;

        // Calculate the start year
        int startYear = currentYear - 100;

        // Create a list to store the results
        List<NumberOfMoviesPerYear> results = new List<NumberOfMoviesPerYear>();

        // Iterate through each year from startYear to currentYear
        for (int year = startYear; year <= currentYear; year++)
        {
            // Query the TMDB API to get the movies released in the current year
            var moviesResponse = await client.DiscoverMoviesAsync().WherePrimaryReleaseIsInYear(year)
                .Query();

            // Get the total number of movies in the response
            int numberOfMovies = moviesResponse.TotalResults;

            // Create a new NumberOfMoviesPerYear object and add it to the results list
            NumberOfMoviesPerYear moviesPerYear = new NumberOfMoviesPerYear
            {
                Year = year,
                NumberOfMovies = numberOfMovies
            };

            results.Add(moviesPerYear);
            await _statisticsRepo.AddMoviesPerYear(moviesPerYear);
        }

        return _mapper.ProjectTo<NumberOfMoviesPerYearDto>(results.AsQueryable()).ToList();
    }
}