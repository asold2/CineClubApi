namespace CineClubApi.Models.Auth;

public class User : Entity
{
    public string Username { get; set; }
    public byte[]? PasswordHash { get; set; }
    public byte[]? PasswordSalt { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? TokenCreated { get; set; }
    public DateTime? TokenExpires { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }

    public ICollection<List> CreatedLists { get; } = new List<List>();
    public ICollection<Tag> CreatedTags { get; } = new List<Tag>();
}