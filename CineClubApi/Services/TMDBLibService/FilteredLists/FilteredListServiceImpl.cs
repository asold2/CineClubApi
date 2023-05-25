using AutoMapper;
using CineClubApi.Common.DTOs.List;
using CineClubApi.Common.DTOs.Movies;
using CineClubApi.Common.Helpers;
using CineClubApi.Models;
using Microsoft.EntityFrameworkCore;
using TMDbLib.Objects.Discover;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;

namespace CineClubApi.Services.TMDBLibService.FilteredLists;

public class FilteredListServiceImpl : TmdbLib, IFilteredListService
{
    public FilteredListServiceImpl(IMapper mapper, IPaginator paginator) : base(mapper, paginator)
    {
    }


    public async Task<PaginatedListOfMovies> GetFilteredListOfMovies(
        int page, int? start, int? end,
        List<int>? genreIds,
        int? year,
        DateTime? releasedAfter,
        DateTime? releasedBefore,
        double? leastVoteAverage,
        string? language,
        DiscoverMovieSortBy? sortBy,
        bool? includeAdultMovies
    )
    {
        var discoverer = client.DiscoverMoviesAsync();


        if (genreIds != null)
        {
            discoverer = discoverer.IncludeWithAllOfGenre(genreIds);
        }
    
        if (releasedAfter != null)
        {
            discoverer = discoverer.WherePrimaryReleaseDateIsAfter((DateTime) releasedAfter);
        }

        if (year != null)
        {
            discoverer = discoverer.WherePrimaryReleaseIsInYear((int) year);

        }

        if (releasedBefore != null)
        {
            discoverer = discoverer.WherePrimaryReleaseDateIsBefore((DateTime) releasedBefore);
        }

        if (leastVoteAverage!=null)
        {
            discoverer = discoverer.WhereVoteAverageIsAtLeast((double) leastVoteAverage);
        }

        if (language!=null)
        {
            discoverer = discoverer.WhereOriginalLanguageIs(language);
        }

        if (sortBy!=null)
        {
            discoverer = discoverer.OrderBy((DiscoverMovieSortBy) sortBy);
        }

        if (includeAdultMovies != null)
        {
            discoverer = discoverer.IncludeAdultMovies((bool) includeAdultMovies);
        }


        var listToReturn = await discoverer.Query(page);
        
        

        var listOfDiscoveredMovies =
            _mapper.ProjectTo<MovieForListDto>(listToReturn.Results.AsQueryable()).ToList();

        var discoveredMovies = new List<MovieForListDto>();

        
        var paginatedList = new PaginatedListOfMovies();

        
        if (start != null && end != null)
        {
            discoveredMovies= await _paginator.PaginateMoviesList(listToReturn, start, page, end);

            paginatedList.Movies = discoveredMovies;
            paginatedList.numberOfPages = listToReturn.TotalPages;
            
            return paginatedList;
        }


        // listOfDiscoveredMovies = await AssignImagesToMovie(listOfDiscoveredMovies, false);
 
        paginatedList.Movies = listOfDiscoveredMovies;
        paginatedList.numberOfPages = listToReturn.TotalPages;
            
        return paginatedList;
        
    }

}