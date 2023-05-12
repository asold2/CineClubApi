using AutoMapper;
using CineClubApi.Common.DTOs.List;
using CineClubApi.Common.DTOs.Movies;
using CineClubApi.Models;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;

namespace CineClubApi.Common.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<List, UpdateListDto>();
        // CreateMap<Movie, MovieDao >();
        // CreateMap<SearchMovie, MovieDao>();
        CreateMap<SearchMovie, MovieForListDto>();
        CreateMap<Movie, MovieForListDto >();
        CreateMap<Movie, DetailedMovieDto >();

    }
}