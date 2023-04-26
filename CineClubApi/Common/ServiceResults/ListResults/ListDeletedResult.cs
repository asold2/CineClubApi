namespace CineClubApi.Common.ServiceResults.ListResults;

public class ListDeletedResult : ServiceResult
{
    public ListDeletedResult()
    {
        Result = "List deleted successfully!";
        StatusCode = 200;
    }
}