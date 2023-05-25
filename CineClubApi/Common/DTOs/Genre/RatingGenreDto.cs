namespace CineClubApi.Common.DTOs.Genre;

public class RatingGenreDto
{
    public TMDbLib.Objects.General.Genre Genre { get; set; }
    public double AvgRating { get; set; }
}