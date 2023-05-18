using TMDbLib.Objects.Discover;

namespace CineClubApi.Common.RequestBody;

public class FilteringRequestBody
{
    public List<int>? GenreIds { get; set; }
    public List<int>?  PeopleIds{ get; set; }
    public int? Year { get; set; }
    public DateTime? ReleasedAfter { get; set; }
    public DateTime? ReleasedBefore { get; set; }
    public double? LeastVoteAverage{ get; set; }
    public string?  Language { get; set; }
    public DiscoverMovieSortBy? sortBy { get; set; }
    public bool? IncludeAdultMovies { get; set; }
}