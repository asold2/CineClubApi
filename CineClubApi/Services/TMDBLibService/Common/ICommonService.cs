using CineClubApi.Common.DTOs.Common;

namespace CineClubApi.Services.TMDBLibService;

public interface ICommonService
{
    Task<List<LanguageDto>> GetAllLanguages();
}