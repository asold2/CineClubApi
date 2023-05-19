using AutoMapper;
using CineClubApi.Common.DTOs.Actor;
using CineClubApi.Common.DTOs.Movies;
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
        
        var personToReturn = _mapper.Map<DetailedPersonInfoDto>(person);
        
        personToReturn.Picture = await GetImageFromPath(person.ProfilePath);


        personToReturn.MoviesPersonTakesPartIn =
            await GetMoviesPersonParticipatesIn(personId, person.KnownForDepartment);

        personToReturn.MoviesPersonTakesPartIn = await AssignImagesToMovie(personToReturn.MoviesPersonTakesPartIn, false);
        
        
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


    public async Task<List<MovieForListDto>> GetMoviesPersonParticipatesIn(int personId, string knownForDepartment)
    {
        var discoverer = client.DiscoverMoviesAsync();

        if (knownForDepartment == "Acting")
        {
            var lsitOfMovies = discoverer.IncludeWithAllOfCast(new List<int>(){personId});
        }
        else
        {
            var listOfMovies = discoverer.IncludeWithAllOfCrew(new List<int>() {personId});
        }

        var list = await discoverer.Query();

        return  _mapper.ProjectTo<MovieForListDto>(list.Results.AsQueryable()).ToList();


    }


}