using CineClubApi.Common.DTOs.Auth;
using CineClubApi.Common.DTOs.User;
using CineClubApi.Common.ServiceResults;
using CineClubApi.Common.ServiceResults.LoginResult;
using CineClubApi.Models.Auth;

namespace CineClubApi.Services.AccountService;

public interface IUserService
{
    public Task<ServiceResult> CreateAccount(AccountDto accountDto, UserDto userDto);
    public Task<ServiceResult> AuthenticateUser(AccountDto accountDto);
    Task LogoutUser(TokenBody tokenBody);
}