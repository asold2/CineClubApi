using CineClubApi.Common.DTOs.Actor;

namespace CineClubApi.Services.TMDBLibService.Actor;

public interface ITMDBPeopleService
{
    Task<List<MoviePersonDto>> GetAllActors(int movieId);
    Task<DetailedPersonInfoDto> GetDetailedInfoAboutPerson(int personId);
    Task<List<MoviePersonDto>> GetMovieCrew(int movieId);
    Task<List<MoviePersonDto>> GetMovieDirectors(int movieId);
}   