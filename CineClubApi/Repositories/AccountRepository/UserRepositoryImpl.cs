using CineClubApi.Common.Interfaces;
using CineClubApi.Models.Auth;
using Microsoft.EntityFrameworkCore;

namespace CineClubApi.Repositories.AccountRepository;

public class UserRepositoryImpl : IUserRepository
{
    private readonly IApplicationDbContext _applicationDbContext;


    public UserRepositoryImpl(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }


    public async Task CreateAccount(User user)
    {
        await _applicationDbContext.Users.AddAsync(user);
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task<IList<User>> GetAllAccounts()
    {
        return await _applicationDbContext.Users.ToListAsync();
    }

    public async Task<User> GetUserByUsername(string username)
    {
        var result = await _applicationDbContext.Users.FirstOrDefaultAsync(x => x.Username == username);
        return result;

    }

    public async Task SetUserRefreshToken(User user, RefreshToken refreshToken)
    {
        user.RefreshToken = refreshToken.Token;
        user.TokenCreated = refreshToken.Created;
        user.TokenExpires = refreshToken.Expires;

        _applicationDbContext.Users.Update(user);
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task LogoutUser(TokenBody tokenBody)
    {
        var userWithRefreshToken =
            await _applicationDbContext.Users.FirstOrDefaultAsync(x => x.RefreshToken == tokenBody.RefreshToken);

        if (userWithRefreshToken == null)
        {
            throw new Exception("Something went wrong");
        }

        userWithRefreshToken.RefreshToken = null;
        userWithRefreshToken.TokenCreated = null;
        userWithRefreshToken.TokenExpires = null;

        _applicationDbContext.Users.Update(userWithRefreshToken);
        await _applicationDbContext.SaveChangesAsync();

    }

    public async Task<User> GetUserByRefreshToken(string tokenBody)
    {
        var neededUser =
            await _applicationDbContext.Users.FirstOrDefaultAsync(x => x.RefreshToken == tokenBody);
        

        return neededUser;

    }

    public async  Task<User> GetUserById(Guid userId)
    {
        var neededUser = await _applicationDbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

        return neededUser;
    }
}