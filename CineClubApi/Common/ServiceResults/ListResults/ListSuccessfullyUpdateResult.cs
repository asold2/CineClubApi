namespace CineClubApi.Common.ServiceResults.ListResults;

public class ListSuccessfullyUpdateResult : ServiceResult
{
    public ListSuccessfullyUpdateResult()
    {
        Result = "List successfully updated!";
        StatusCode = 200;
    }
    
}