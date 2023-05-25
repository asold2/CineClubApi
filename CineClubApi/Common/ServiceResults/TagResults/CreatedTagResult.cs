namespace CineClubApi.Common.ServiceResults.TagResults;

public class CreatedTagResult : ServiceResult
{

    public Guid tagId { get; set; }

    public CreatedTagResult()
    {
        Result = "Created tag successfully";
        StatusCode = 200;
    }
    
    
}