namespace CineClubApi.Models;

public class MovieDao 
{
    public Guid Id { get; set; }
    public int tmdbId { get; set; }
    public ICollection<List> Lists { get; set; } = new List<List>();
}