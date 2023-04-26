using CineClubApi.Models.Auth;

namespace CineClubApi.Models;

public class List : Entity
{
    public string Name { get; set; }
    public bool Public { get; set; }
    public Guid CreatorId { get; set; }
    public User Creator { get; set; }
}
