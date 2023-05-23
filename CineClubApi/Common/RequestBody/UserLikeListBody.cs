namespace CineClubApi.Common.RequestBody;

public class UserLikeListBody
{
    public Guid UserId { get; set; }
    public Guid ListId { get; set; }
}