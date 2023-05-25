using CineClubApi.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CineClubApi.Services.AccountService;

public class AuthServiceImpl : IAuthService
{

    private readonly IApplicationDbContext _applicationDbContext;

    public AuthServiceImpl(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }
    
    
    public async Task<bool> ValidateToken(string token)
    {
        if (token.IsNullOrEmpty())
        {
            return false;
        }
        
        if (await _applicationDbContext.Users.AnyAsync(x => x.RefreshToken == token))
        {
            return true;
        }

        return false;
    }


}