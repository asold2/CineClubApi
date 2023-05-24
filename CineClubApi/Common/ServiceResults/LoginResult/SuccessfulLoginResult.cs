using CineClubApi.Models.Auth;

namespace CineClubApi.Common.ServiceResults.LoginResult;

public class SuccessfulLoginResult : ServiceResult
{

    public string Token { get; set; }
    public Guid UserId { get; set; }
    public SuccessfulLoginResult()
    {
        
        StatusCode = 200;
    }
}