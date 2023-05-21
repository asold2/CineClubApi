using CineClubApi.Common.DTOs.Common;
using CineClubApi.Services.MovieService;
using CineClubApi.Services.TMDBLibService;
using Microsoft.AspNetCore.Mvc;

namespace CineClubApi.Controllers;

public class CommonController : CineClubControllerBase
{

    private readonly ICommonService _commonService;
    private readonly IMovieService _movieService;

    public CommonController(ICommonService commonService, IMovieService movieService)
    {
        _commonService = commonService;
        _movieService = movieService;
    }
    
    
    [HttpGet("language")]
    public async Task<List<LanguageDto>> GetAllLanguages()
    {
        return await _commonService.GetAllLanguages();
    }

    [HttpGet("image")]
    public async Task<byte[]> GetImageByUrl([FromQuery] string url)
    {
        var result = await _commonService.GetImageFromPath(url);
        return result;


    }

}