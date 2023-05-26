namespace CineClubApi.Models.Statistics;

public class RatingPerDirector : Entity
{
    public int TmdbId { get; set; }
    public string Name  { get; set; }
    public float Rating { get; set; }
}