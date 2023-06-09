﻿using CineClubApi.Common.DTOs.Auth;
using CineClubApi.Common.DTOs.User;
using CineClubApi.Common.ServiceResults;
using CineClubApi.Common.ServiceResults.AccountResults;
using CineClubApi.Common.ServiceResults.LoginResult;
using CineClubApi.Models;
using CineClubApi.Models.Auth;
using CineClubApi.Repositories.AccountRepository;
using CineClubApi.Repositories.ListRepository;
using CineClubApi.Services.TokenService;
using Microsoft.IdentityModel.Tokens;

namespace CineClubApi.Services.AccountService;

public class UserServiceImpl : IUserService
{

    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly ITokenService _tokenService;
    private readonly IListRepository _listRepository;

    public UserServiceImpl(IUserRepository userRepository, IPasswordService passwordService, ITokenService tokenService, IListRepository listRepository)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _tokenService = tokenService;
        _listRepository = listRepository;
    }
    
    
    public async Task<ServiceResult> CreateAccount(AccountDto accountDto, UserDto userDto)
    {
        var allAccounts = await _userRepository.GetAllAccounts();

        if (allAccounts.Any(x => x.Username == accountDto.Username ))
        {
            return new UsernameExistsResult();
        }

        if (accountDto.Password.IsNullOrEmpty() || accountDto.Username.IsNullOrEmpty() || userDto.Email.IsNullOrEmpty())
        {
            return new ServiceResult
            {
                Result = "Cannot leave username, password or email empty",
                StatusCode = 400
            };
        }

        if (allAccounts.Any(x => x.Email == userDto.Email ))
        {
            return new EmailExistsResult();
        }
        
        _passwordService.CreatePasswordHash(accountDto.Password, out byte[] passwordHash,
            out byte[] passwordSalt);
        
        var newAccount = new User()
        {
            Username = accountDto.Username,
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            Email = userDto.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };

        await _userRepository.CreateAccount(newAccount);


        var likedList = new List
        {
            Name = "Liked Movies",
            Public = false,
            CreatorId = newAccount.Id,
            Creator = newAccount
        };
        
        
        var watchedList = new List
        {
            Name = "Watched Movies",
            Public = false,
            CreatorId = newAccount.Id,
            Creator = newAccount
        };

        await _listRepository.CreateList(likedList);
        await _listRepository.CreateList(watchedList);

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
                Result = "Username not found!"
            };
        }

        var user = await _userRepository.GetUserByUsername(accountDto.Username);


        if (!_passwordService.VerifyPasswordHash(accountDto.Password, user.PasswordHash, user.PasswordSalt))
        {
            return new ServiceResult
            {
                StatusCode = 400,
                Result = "Wrong Password!"
            };
        }


        // string token = _tokenService.CreateToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        await _userRepository.SetUserRefreshToken(user, refreshToken);

        return new SuccessfulLoginResult
        {

            Result = refreshToken.Token,
            StatusCode = 200
            
        };

    }

    public async Task LogoutUser(TokenBody tokenBody)
    {
        await _userRepository.LogoutUser(tokenBody);
    }

    public async Task<Guid> GetUserId(string refreshToken)
    {
        var neededUser = await _userRepository.GetUserByRefreshToken(refreshToken);
        return neededUser.Id;
        
    }
}