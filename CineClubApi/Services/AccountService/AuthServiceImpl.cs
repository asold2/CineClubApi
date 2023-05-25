using CineClubApi.Common.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace CineClubApi.Services.AccountService;

public class AuthServiceImpl : IAuthService
{

    private readonly IApplicationDbContext _applicationDbContext;

    public AuthServiceImpl(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }
    
    
    public  bool ValidateToken(string token)
    {
        if (token.IsNullOrEmpty())
        {
            return false;
        }
        
        if (_applicationDbContext.Users.Any(x => x.RefreshToken == token))
        {
            return true;
        }

        return false;
    }


}