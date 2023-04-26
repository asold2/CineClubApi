using CineClubApi.Common.DTOs.Auth;
using CineClubApi.Common.DTOs.User;
using CineClubApi.Common.ServiceResults;
using CineClubApi.Common.ServiceResults.AccountResults;
using CineClubApi.Common.ServiceResults.LoginResult;
using CineClubApi.Models.Auth;
using CineClubApi.Repositories.AccountRepository;
using CineClubApi.Services.TokenService;

namespace CineClubApi.Services.AccountService;

public class UserServiceImpl : IUserService
{

    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly ITokenService _tokenService;

    public UserServiceImpl(IUserRepository userRepository, IPasswordService passwordService, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _tokenService = tokenService;
    }
    
    
    public async Task<ServiceResult> CreateAccount(AccountDto accountDto, UserDto userDto)
    {
        var allAccounts = await _userRepository.GetAllAccounts();

        if (allAccounts.Any(x => x.Username == accountDto.Username))
        {
            return new AccountExistsResult();
        }
        
        _passwordService.CreatePasswordHash(accountDto.Password, out byte[] passwordHash,
            out byte[] passwordSalt);
        
        var newAccount = new User()
        {
            Username = accountDto.Username,
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            Email = userDto.LastName,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };

        await _userRepository.CreateAccount(newAccount);

        return new CreatedAccountResult();
    }

    public  async Task<ServiceResult> AuthenticateUser(AccountDto accountDto)
    {
        var accounts = await _userRepository.GetAllAccounts();
        
        if (!accounts.Any(x => x.Username == accountDto.Username))
        {
            return new ServiceResult
            {
                StatusCode = 400,
                Result = "Username not found"
            };
        }

        var user = await _userRepository.GetUserByUsername(accountDto.Username);


        if (!_passwordService.VerifyPasswordHash(accountDto.Password, user.PasswordHash, user.PasswordSalt))
        {
            return new ServiceResult
            {
                StatusCode = 400,
                Result = "Wrong Password"
            };
        }


        // string token = _tokenService.CreateToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        await _userRepository.SetUserRefreshToken(user, refreshToken);

        return new SuccessfulLoginResult
        {
            TokenBody = new TokenBody
            {
                RefreshToken = refreshToken.Token
            },
            StatusCode = 200
            
        };

    }

    public async Task LogoutUser(TokenBody tokenBody)
    {
        await _userRepository.LogoutUser(tokenBody);
    }
}