namespace CineClubApi.Common.ServiceResults.AccountResults;

public class UsernameExistsResult : ServiceResult
{
    public UsernameExistsResult()
    {
        Result = "Account with this username already exists";
        StatusCode = 409;
    }
}