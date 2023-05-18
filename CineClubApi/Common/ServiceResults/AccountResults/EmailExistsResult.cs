namespace CineClubApi.Common.ServiceResults.AccountResults;

public class EmailExistsResult : ServiceResult
{
    public EmailExistsResult()
    {
        Result = "Account with this email already exists!";
        StatusCode = 409;
    }
}