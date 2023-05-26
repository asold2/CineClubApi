using CineClubApi.Models;

namespace CineClubApi.Common.DTOs.Genre;

public class RatingGenreDto : Entity
{
    public TMDbLib.Objects.General.Genre Genre { get; set; }
    public double AvgRating { get; set; }
}