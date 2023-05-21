namespace CineClubApi.Common.RequestBody;

public class TagBody
{
    public string Name { get; set; }
    public Guid  UserId { get; set; }
    public Guid ListId { get; set; }
    
}