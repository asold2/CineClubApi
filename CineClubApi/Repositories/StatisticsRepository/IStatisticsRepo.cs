using CineClubApi.Common.DTOs.Genre;

namespace CineClubApi.Repositories.StatisticsRepository;

public interface IStatisticsRepo
{
    Task SaveGenreStatistic(RatingGenreDto genreDto);
    Task<List<RatingGenreDto>> GetGenreStatistics();
    Task<bool> GenreratingsNeedUpdated();
}