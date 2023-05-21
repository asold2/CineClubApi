using System.Transactions;
using AutoMapper;
using CineClubApi.Common.DTOs.Movies;
using CineClubApi.Common.Enums;
using CineClubApi.Common.Helpers;
using CineClubApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.Trending;

namespace CineClubApi.Services.TMDBLibService;

public class ItmdbMovieServiceImpl :TmdbLib, ITMDBMovieService 
{
    
    public ItmdbMovieServiceImpl(IMapper mapper, IPaginator paginator) : base( mapper, paginator)
    {
    }

    public async Task<List<MovieForListDto>> GetMoviesByKeyword(string keyword)
    {
        var result =  client.SearchMovieAsync(keyword, 0, true, 0, null, 0).Result.Results.AsQueryable();
        var movieDtos =  _mapper.ProjectTo<MovieForListDto>(result).ToList();

        // movieDtos = await AssignImagesToMovie(movieDtos, false);
        
        return movieDtos;
    }
    

    public async Task<DetailedMovieDto> getMovieById(int id)
    {
        var result = await client.GetMovieAsync(id);

        byte[] movieBackdrop = await GetImageFromPath(result.BackdropPath);
        byte[] poster = await GetImageFromPath(result.PosterPath);
        
        
        var movieDto =  _mapper.Map<DetailedMovieDto>(result);
        // movieDto.Poster = poster;
        // movieDto.Backdrop = movieBackdrop;
        return movieDto;
    }



    
    

   


}