namespace CineClubApi.Common.ServiceResults.AccountResults;

public class CreatedAccountResult : ServiceResult
{
    public CreatedAccountResult()
    {
        Result = "Account created successfully";
        StatusCode = 200;
    }
}