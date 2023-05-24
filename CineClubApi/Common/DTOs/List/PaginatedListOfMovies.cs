using CineClubApi.Common.DTOs.Movies;

namespace CineClubApi.Common.DTOs.List;

public class PaginatedListOfMovies
{
    public List<MovieForListDto> Movies { get; set; }
    public int  numberOfPages { get; set; }
}