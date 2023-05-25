namespace CineClubApi.Services.AccountService;

public interface IAuthService
{
    bool ValidateToken(string token);
}