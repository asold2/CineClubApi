using CineClubApi.Models.Auth;

namespace CineClubApi.Common.DTOs.List;

public class ListDto
{
    public string Name { get; set; }
    public bool Public { get; set; }

    public Guid UserId { get; set; }
    
    // public TokenBody TokenBody { get; set; }
}   