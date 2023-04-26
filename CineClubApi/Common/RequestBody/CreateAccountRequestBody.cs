using CineClubApi.Common.DTOs.Auth;
using CineClubApi.Common.DTOs.User;

namespace CineClubApi.Common.RequestBody;

public class CreateAccountRequestBody
{
    public AccountDto AccountDto { get; set; }
    public UserDto UserDto { get; set; }
}