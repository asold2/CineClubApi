using CineClubApi.Models;

namespace CineClubApi.Common.DTOs.Statistics;

public class NumberOfMoviesPerYear : Entity
{
    public int Year { get; set; }
    public int NumberOfMovies { get; set; }
}