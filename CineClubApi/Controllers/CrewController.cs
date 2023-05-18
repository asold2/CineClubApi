using CineClubApi.Common.DTOs.Actor;
using CineClubApi.Models;
using CineClubApi.Services.TMDBLibService.Actor;
using Microsoft.AspNetCore.Mvc;

namespace CineClubApi.Controllers;

public class ActorController : CineClubControllerBase
{
    private readonly ITMDBPeopleService _peopleService;

    public ActorController(ITMDBPeopleService peopleService)
    {
        _peopleService = peopleService;
    }

    [HttpGet("cast/{movieId:int}")]
    public async Task<List<MoviePersonDto>> GetCreditsForMovie([FromRoute] int movieId)
    {
        return  await _peopleService.GetAllActors(movieId);
    }

    [HttpGet("person/{personId:int}")]
    public async Task<DetailedPersonInfoDto> GetPersonInfo([FromRoute] int personId)
    {
        return await _peopleService.GetDetailedInfoAboutPerson(personId);
    }

    [HttpGet("crew/{movieId:int}")]
    public async Task<List<MoviePersonDto>> GetMovieCrew([FromRoute] int movieId)
    {
        return await _peopleService.GetMovieCrew(movieId);
    }

}