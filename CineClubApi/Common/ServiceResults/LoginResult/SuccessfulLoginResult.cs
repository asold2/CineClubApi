using CineClubApi.Models.Auth;

namespace CineClubApi.Common.ServiceResults.LoginResult;

public class SuccessfulLoginResult : ServiceResult
{
    public TokenBody TokenBody { get; set; }

    public SuccessfulLoginResult()
    {
        StatusCode = 200;
    }
}