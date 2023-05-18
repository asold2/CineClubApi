using AutoMapper;
using CineClubApi.Common.DTOs.Actor;
using CineClubApi.Common.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CineClubApi.Services.TMDBLibService.Actor;

public class ItmdbPeopleServiceImpl : TmdbLib, ITMDBPeopleService
{
    public ItmdbPeopleServiceImpl(IMapper mapper, IPaginator paginator) : base(mapper, paginator)
    {
    }

    public async Task<List<MoviePersonDto>> GetAllActors(int movieId)
    {
        var movieCredits = client.GetMovieCreditsAsync(movieId).Result.Cast.AsQueryable();
        var movieCast = _mapper.ProjectTo<MoviePersonDto>(movieCredits).ToList();
        return movieCast;
    }

    public async Task<DetailedPersonInfoDto> GetDetailedInfoAboutPerson(int personId)
    {
        var person = await client.GetPersonAsync(personId);
        
        // var personImage = await client.GetPersonImagesAsync(personId);
        
        var personToReturn = _mapper.Map<DetailedPersonInfoDto>(person);
        
        personToReturn.Picture = await GetImageFromPath(person.ProfilePath);
        
        return personToReturn;
    }

    public  async Task<List<MoviePersonDto>> GetMovieCrew(int movieId)
    {
        var crew = client.GetMovieCreditsAsync(movieId).Result.Crew
            .Where(x=>x.Job=="Producer" || x.Job=="Director" || x.Job=="Writer" || x.Job=="Composer")
            .AsQueryable();

        var crewToReturn = _mapper.ProjectTo<MoviePersonDto>(crew).ToList();

        return crewToReturn;

    }


}