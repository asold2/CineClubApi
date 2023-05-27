namespace CineClubApi.Common.DTOs.Movies;

public class MovieForListDto
{
    public long Budget { get; set; }
    public long Revenue { get; set; }

    public int Id { get; set; }
    // public List<TMDbLib.Objects.General.Genre> Genres { get; set; }
    public List<int> GenreIds { get; set; }
    public string OriginalLanguage { get; set; }
    public string OriginalTitle { get; set; }
    public string Title { get; set; }
    public double Popularity { get; set; }
    public double VoteAverage { get; set; }
    public int VoteCount { get; set; }
    public string BackdropPath  { get; set; }
    public string PosterPath  { get; set; }
    // public byte[] BackdropImage { get; set; }
    // public byte[] PosterImage { get; set; }
    public DateTime ReleaseDate { get; set; }
  
}