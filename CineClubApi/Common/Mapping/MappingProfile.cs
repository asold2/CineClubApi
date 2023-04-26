using AutoMapper;
using CineClubApi.Common.DTOs.List;
using CineClubApi.Models;

namespace CineClubApi.Common.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<List, UpdateListDto>();
    }
}