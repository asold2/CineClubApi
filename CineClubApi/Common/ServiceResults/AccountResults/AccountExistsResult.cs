namespace CineClubApi.Common.ServiceResults.AccountResults;

public class AccountExistsResult : ServiceResult
{
    public AccountExistsResult()
    {
        Result = "Account with this username already exists";
        StatusCode = 409;
    }
}