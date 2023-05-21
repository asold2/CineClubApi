using CineClubApi.Common.DTOs.Movies;

namespace CineClubApi.Common.DTOs.Actor;

public class DetailedPersonInfoDto
{
    public string Biography { get; set; }
    public DateTime Birthday { get; set; }
    public DateTime Deathday { get; set; }
    public int Id { get; set; }
    public string KnownForDepartment { get; set; }
    public string Name { get; set; }
    public string PlaceOfBirth { get; set; }
    public double Popularity  { get; set; }
    public string ProfilePath { get; set; }
    public byte[] Picture { get; set; }

    public List<MovieForListDto> MoviesPersonTakesPartIn { get; set; }
    
}