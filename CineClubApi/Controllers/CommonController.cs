using CineClubApi.Common.DTOs.Common;
using CineClubApi.Services.TMDBLibService;
using Microsoft.AspNetCore.Mvc;

namespace CineClubApi.Controllers;

public class CommonController : CineClubControllerBase
{

    private readonly ICommonService _commonService;

    public CommonController(ICommonService commonService)
    {
        _commonService = commonService;
    }
    
    
    [HttpGet("language")]
    public async Task<List<LanguageDto>> GetAllLanguages()
    {
        return await _commonService.GetAllLanguages();
    }

}