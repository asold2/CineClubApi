using CineClubApi.Models.Auth;

namespace CineClubApi.Services.TokenService;

public interface ITokenService
{
    public string CreateToken(User user);
    public RefreshToken GenerateRefreshToken();
}