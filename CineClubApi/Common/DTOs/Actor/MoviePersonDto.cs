namespace CineClubApi.Common.DTOs.Actor;

public class MoviePersonDto
{
    public string Character { get; set; }
    public int Id { get; set; }
    public string KnownForDepartment { get; set; }
    public string Name { get; set; }
    public string OriginalName { get; set; }
    public float Popularity { get; set; }
    public string Job { get; set; }
}