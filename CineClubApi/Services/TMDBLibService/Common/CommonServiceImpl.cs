using AutoMapper;
using CineClubApi.Common.DTOs.Common;
using CineClubApi.Common.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CineClubApi.Services.TMDBLibService;

public class CommonServiceImpl : TmdbLib, ICommonService
{
    public CommonServiceImpl(IMapper mapper, IPaginator paginator) : base(mapper, paginator)
    {
    }

    
    
    public async Task<List<LanguageDto>> GetAllLanguages()
    {
        var listOfLanguages = await client.GetLanguagesAsync();
        
        var listToReturn =  _mapper.Map<List<LanguageDto>>(listOfLanguages);

        return listToReturn;


    }
}