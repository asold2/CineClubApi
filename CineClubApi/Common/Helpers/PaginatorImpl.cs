﻿using AutoMapper;
using CineClubApi.Common.DTOs.Movies;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

namespace CineClubApi.Common.Helpers;

public class PaginatorImpl : IPaginator
{
    
    private readonly IMapper _mapper;

    public PaginatorImpl(IMapper mapper)
    {
        _mapper = mapper;
    }
    
    
    
    public async Task<List<MovieForListDto>> PaginateMoviesList(SearchContainer<SearchMovie> list, int? start, int page, int? end)
    {
        int pageSize = (int)end - (int)start + 1;

        var totalPages = list.TotalPages;

        if (page < 1 || page > totalPages)
        {
            return new List<MovieForListDto>();
        }

        var moviesToTake = list.Results.Skip((int)start - 1).Take(pageSize).AsQueryable();
        
        var popularMoviesList = _mapper.ProjectTo<MovieForListDto>(moviesToTake).ToList();

        return popularMoviesList;
    }
}