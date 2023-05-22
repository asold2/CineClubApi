namespace CineClubApi.Common.RequestBody;

public class DeleteTagBody
{
    public Guid TagId { get; set; }
    public Guid UserId { get; set; }
}