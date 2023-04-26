using CineClubApi.Models.Auth;

namespace CineClubApi.Repositories.AccountRepository;

public interface IUserRepository
{
    public  Task CreateAccount(User user);
    public Task<IList<User>> GetAllAccounts();

    public Task<User> GetUserByUsername(string username);

    Task SetUserRefreshToken(User user, RefreshToken refreshToken);

    public Task LogoutUser(TokenBody tokenBody);

}