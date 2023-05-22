using AutoMapper;
using CineClubApi.Common.DTOs.Actor;
using CineClubApi.Common.DTOs.Common;
using CineClubApi.Common.DTOs.List;
using CineClubApi.Common.DTOs.Movies;
using CineClubApi.Common.DTOs.Tag;
using CineClubApi.Models;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Languages;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.People;
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
        CreateMap<Cast, MoviePersonDto>();
        CreateMap<Person, DetailedPersonInfoDto>();
        CreateMap<Crew, MoviePersonDto>();
        CreateMap<Language, LanguageDto>();
        CreateMap<Tag, TagForListDto>();
        CreateMap<Tag, TagDto>();
        CreateMap<List, ListDto>();

    }
}