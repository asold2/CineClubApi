namespace CineClubApi.Common.RequestBody;

public class AssignTagToListBody
{
    public Guid TagId { get; set; }
    public Guid  UserId { get; set; }
    public Guid ListId { get; set; }
}