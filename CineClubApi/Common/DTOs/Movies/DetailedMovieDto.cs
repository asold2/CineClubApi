using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Reviews;

namespace CineClubApi.Common.DTOs.Movies;

public class DetailedMovieDto
{
    public long Budget { get; set; }
    public IList<TMDbLib.Objects.General.Genre> Genres { get; set; }
    public string Homepage { get; set; }
    public int Id { get; set; }
    public string ImdbId  { get; set; }
    public Images Images { get; set; }
    public KeywordsContainer  KeywordsContainer { get; set; }
    public string OriginalLanguage { get; set; }
    public string OriginalTitle { get; set; }
    public string Overview { get; set; }
    public double Popularity { get; set; }
    // public byte[] Backdrop { get; set; }
    // public byte[] Poster { get; set; }
    public IList<ProductionCompany> ProductionCompanies { get; set; }
    public IList<ProductionCountry> ProductionCountries { get; set; }
    public DateTime ReleaseDate { get; set; }
    public long Revenue { get; set; }
    public List<Review> Reviews { get; set; }
    public int Runtime { get; set; }
    public string Status { get; set; }
    public string Tagline { get; set; }
    public string Title { get; set; }
    public double VoteAverage { get; set; }
    public int VoteCount { get; set; }
    public string BackdropPath  { get; set; }
    public string PosterPath  { get; set; }
 
}