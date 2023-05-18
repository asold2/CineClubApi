using CineClubApi.Models.Auth;

namespace CineClubApi.Common.ServiceResults.LoginResult;

public class SuccessfulLoginResult : ServiceResult
{

    public SuccessfulLoginResult()
    {
        
        StatusCode = 200;
    }
}