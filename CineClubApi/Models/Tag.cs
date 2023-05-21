
using CineClubApi.Models.Auth;

namespace CineClubApi.Models;

public class Tag : Entity
{
    public string Name { get; set; }
    public User Creator { get; set; }
    public Guid CreatorId { get; set; }
    public ICollection<List> Lists { get; set; } = new List<List>();
}