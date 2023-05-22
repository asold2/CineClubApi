using AutoMapper;
using CineClubApi.Common.DTOs.List;
using CineClubApi.Common.DTOs.Movies;
using CineClubApi.Models;
using Microsoft.EntityFrameworkCore;
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

    public async Task<List<ListDto>> PaginateLists(List<ListDto> list, int? page, int? start, int? end)
    {
        int pageSize = (int)end - (int)start + 1;

        if (page < 1 )
        {
            return new List<ListDto>();
        }

        var listsToTake = list.Skip((int)start - 1).Take(pageSize).AsQueryable();
        
        // var lists = _mapper.ProjectTo<ListDto>(listsToTake).ToList();

        return  listsToTake.ToList();
    }
}