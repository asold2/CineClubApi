using CineClubApi.Models.Auth;

namespace CineClubApi.Models;

public class Like : Entity
{
    public User User { get; set; }
    public Guid UserId { get; set; }
    public List List { get; set; }
    public Guid ListId { get; set; }
}