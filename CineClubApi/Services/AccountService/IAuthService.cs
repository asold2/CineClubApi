namespace CineClubApi.Services.AccountService;

public interface IAuthService
{
    Task<bool> ValidateToken(string token);
}