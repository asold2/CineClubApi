using AutoMapper;
using CineClubApi.Common.DTOs.Actor;
using CineClubApi.Common.DTOs.Genre;
using CineClubApi.Common.DTOs.Movies;
using CineClubApi.Common.DTOs.Statistics;
using CineClubApi.Common.Helpers;
using CineClubApi.Models.Statistics;
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

    public async Task<List<MoviePersonDto>> GetMostPopularActors()
    {
        var popularPersons = await client.GetPersonPopularListAsync();

        var popularActors = new List<MoviePersonDto>();

        foreach (var person in popularPersons.Results)
        {
            var detailedPerson = await client.GetPersonAsync(person.Id);

            if (detailedPerson.KnownForDepartment == "Acting")
            {
                var moviePersonDto = _mapper.Map<MoviePersonDto>(detailedPerson);
                popularActors.Add(moviePersonDto);
            }
        }

        popularActors = popularActors
            .OrderByDescending(person => person.Popularity)
            .Take(15)
            .ToList();

        return popularActors;
    }

    public async Task<List<MoviePersonDto>> GetAverageRatingPerDirector()
    {
       var directoratings = await _statisticsRepo.GetRatingPerDirectors();

        if (directoratings.Count != 0)
        {
            var listToReturn = new List<MoviePersonDto>();

            foreach (var director in directoratings)
            {
                var person = await client.GetPersonAsync(director.TmdbId);

                var personToReturn = _mapper.Map<MoviePersonDto>(person);

                personToReturn.Popularity = director.Rating;
                personToReturn.Id = director.TmdbId;
                personToReturn.Name = director.Name;

                listToReturn.Add(personToReturn);
            }

            return listToReturn;
        }

        const int pageSize = 20; // Results per page
        const int totalPeople = 31; // Total number of people to retrieve

        var popularDirectors = new List<MoviePersonDto>();
        var currentPage = 1;

        while (popularDirectors.Count < totalPeople)
        {
            var retryCount = 0;
            var success = false;

            while (!success && retryCount < 5) // Retry up to 5 times
            {
                try
                {
                    var popularPersons = await client.GetPersonPopularListAsync(page: currentPage);

                    foreach (var person in popularPersons.Results)
                    {
                        var detailedPerson = await client.GetPersonAsync(person.Id);

                        if (detailedPerson.KnownForDepartment == "Directing")
                        {
                            var moviePersonDto = _mapper.Map<MoviePersonDto>(detailedPerson);
                            popularDirectors.Add(moviePersonDto);
                        }

                        if (popularDirectors.Count == totalPeople)
                            break;
                    }

                    success = true; // Set success flag to true if no exception occurs
                }
                catch (Exception ex)
                {
                    // Log the exception
                    Console.WriteLine($"An error occurred while retrieving popular persons: {ex}");
                    Console.WriteLine("On retry count: " + retryCount);
                    Console.WriteLine("At time: " + DateTime.UtcNow);
                    retryCount++;
                }
            }

            currentPage++;
        }

        var directorRatings = new Dictionary<string, DirectorRating>();

        foreach (var director in popularDirectors)
        {
            var moviesDirected = await client.GetPersonMovieCreditsAsync(director.Id);

            foreach (var movieJob in moviesDirected.Crew)
            {
                if (movieJob.Job == "Director")
                {
                    var movie = await client.GetMovieAsync(movieJob.Id);

                    if (directorRatings.ContainsKey(director.Name))
                    {
                        directorRatings[director.Name].Ratings.Add(movie.VoteAverage);
                    }
                    else
                    {
                        var directorRating = new DirectorRating
                        {
                            Id = director.Id,
                            Name = director.Name,
                            Ratings = new List<double> { movie.VoteAverage }
                        };
                        directorRatings[director.Name] = directorRating;
                    }
                }
            }
        }

        foreach (var rating in directorRatings)
        {
            var directorToSave = new RatingPerDirector
            {
                TmdbId = rating.Value.Id,
                Name = rating.Key,
                Rating = (float)rating.Value.Ratings.Average()
            };

            await _statisticsRepo.AddRatingPerDirector(directorToSave);
        }

        var topDirectors = directorRatings
            .OrderByDescending(kvp => kvp.Value.Ratings.Average())
            .Take(15)
            .Select(kvp => new MoviePersonDto
            {
                Id = kvp.Value.Id,
                Name = kvp.Key,
                Popularity = (float)kvp.Value.Ratings.Average()
            })
            .ToList();

        return topDirectors;
    }
        
        
        
        
        // const int pageSize = 20; // Results per page
        // const int totalPeople = 500; // Total number of people to retrieve
        //
        // var popularDirectors = new List<MoviePersonDto>();
        // var currentPage = 1;
        //
        // while (popularDirectors.Count < totalPeople)
        // {
        //     var popularPersons = await client.GetPersonPopularListAsync(page: currentPage);
        //     
        //     foreach (var person in popularPersons.Results)
        //     {
        //         var detailedPerson = await client.GetPersonAsync(person.Id);
        //
        //         if (detailedPerson.KnownForDepartment == "Directing")
        //         {
        //             var moviePersonDto = _mapper.Map<MoviePersonDto>(detailedPerson);
        //             popularDirectors.Add(moviePersonDto);
        //             
        //             
        //         }
        //
        //         if (popularDirectors.Count == totalPeople)
        //             break;
        //     }
        //
        //     currentPage++;
        // }
        //
        // var directorRatings = new Dictionary<string, List<double>>();
        //
        // foreach (var director in popularDirectors)
        // {
        //     var moviesDirected = await client.GetPersonMovieCreditsAsync(director.Id);
        //
        //     foreach (var movieJob in moviesDirected.Crew)
        //     {
        //         if (movieJob.Job == "Director")
        //         {
        //             var movie = await client.GetMovieAsync(movieJob.Id);
        //
        //             if (directorRatings.ContainsKey(director.Name))
        //             {
        //                 directorRatings[director.Name].Add(movie.VoteAverage);
        //             }
        //             else
        //             {
        //                 directorRatings[director.Name] = new List<double> { movie.VoteAverage };
        //             }
        //         }
        //     }
        // }
        //
        //
        //
        // var topDirectors = directorRatings
        //     .OrderByDescending(kvp => kvp.Value.Average())
        //     .Take(15)
        //     .Select(kvp => new MoviePersonDto
        //     {
        //         Name = kvp.Key,
        //         Popularity = (float)kvp.Value.Average()
        //     })
        //     .ToList();
        //
        // return topDirectors;
    
}

public class DirectorRating
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<double> Ratings { get; set; }
}