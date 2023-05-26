using CineClubApi.Common.DTOs.Genre;
using CineClubApi.Common.DTOs.Statistics;
using CineClubApi.Models.Statistics;

namespace CineClubApi.Repositories.StatisticsRepository;

public interface IStatisticsRepo
{
    Task SaveGenreStatistic(RatingGenreDto genreDto);
    Task<List<RatingGenreDto>> GetGenreStatistics();
    Task<bool> GenreratingsNeedUpdated();

    Task AddMoviesPerYear(NumberOfMoviesPerYear moviesPerYear);
    Task<List<NumberOfMoviesPerYear>> GetNumberOfMoviesPerYear();
    Task AddRatingPerDirector(RatingPerDirector ratingPerDirector);
    Task<List<RatingPerDirector>> GetRatingPerDirectors();


}