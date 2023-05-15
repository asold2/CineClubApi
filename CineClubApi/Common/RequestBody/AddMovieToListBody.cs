namespace CineClubApi.Common.RequestBody;

public class AddMovieToListBody
{
    public Guid ListId { get; set; }
    public int TmdbId { get; set; }
    
}