using System.Text;
using CineClubApi.Common.DTOs.Auth;
using CineClubApi.Common.DTOs.User;
using CineClubApi.Common.Interfaces;
using CineClubApi.Common.ServiceResults.AccountResults;
using CineClubApi.Models;
using CineClubApi.Models.Auth;
using CineClubApi.Repositories.AccountRepository;
using CineClubApi.Repositories.ListRepository;
using CineClubApi.Services;
using CineClubApi.Services.AccountService;
using CineClubApi.Services.TokenService;
using Moq;
using Xunit;

namespace CineClubApiTests.Services.AccountServices;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<IPasswordService> _passwordService;
    private readonly Mock<ITokenService> _tokenService;
    private readonly Mock<IListRepository> _listRepository;
    private readonly IUserService _userService;

    public UserServiceTests()
    {
        _userRepository = new Mock<IUserRepository>();
        _passwordService = new Mock<IPasswordService>();
        _tokenService = new Mock<ITokenService>();
        _listRepository = new Mock<IListRepository>();
        _userService = new UserServiceImpl(
            _userRepository.Object, _passwordService.Object, _tokenService.Object, _listRepository.Object);
    }

    [Fact]
    public async Task CreateAccount_WithValidData_ReturnsCreatedAccountResult()
    {
        var accountDto = new AccountDto
        {
            Username = "testuser",
            Password = "password"
        };
        var userDto = new UserDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com"
        };

        _userRepository.Setup(x => x.GetAllAccounts()).ReturnsAsync(new List<User>());

        var result = await _userService.CreateAccount(accountDto, userDto);

        Assert.IsType<CreatedAccountResult>(result);
    }
    
    [Fact]
    public async Task CreateAccount_WithExistingUsername_ReturnsUsernameExistsResult()
    {
        var accountDto = new AccountDto
        {
            Username = "existinguser",
            Password = "password"
        };
        var userDto = new UserDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com"
        };

        var existingUser = new User { Username = "existinguser" };
        _userRepository.Setup(x => x.GetAllAccounts()).ReturnsAsync(new List<User> { existingUser });

        var result = await _userService.CreateAccount(accountDto, userDto);

        Assert.IsType<UsernameExistsResult>(result);
    }
    
    
[Fact]
public async Task CreateAccount_UserHasLikedAndWatchedList()
{
    var accountDto = new AccountDto
    {
        Username = "testuser",
        Password = "password"
    };
    var userDto = new UserDto
    {
        FirstName = "John",
        LastName = "Doe",
        Email = "john@example.com"
    };

    var existingAccounts = new List<User>
    {
        new User { Username = "existinguser", Email = "existing@example.com" }
    };
    _userRepository.Setup(x => x.GetAllAccounts()).ReturnsAsync(existingAccounts);

    User createdUser = null;
    _userRepository.Setup(x => x.CreateAccount(It.IsAny<User>()))
        .Callback<User>(user =>
        {
            createdUser = user;
        });

    List<List> createdLists = new List<List>(); 
    _listRepository.Setup(x => x.CreateList(It.IsAny<List>()))
        .Callback<List>(list =>
        {
            createdLists.Add(list);
        });

    var result = await _userService.CreateAccount(accountDto, userDto);


    Assert.IsType<CreatedAccountResult>(result);
    Assert.NotNull(createdUser);
    Assert.Equal(accountDto.Username, createdUser.Username);
    Assert.Equal(userDto.FirstName, createdUser.FirstName);
    Assert.Equal(userDto.LastName, createdUser.LastName);
    Assert.Equal(userDto.Email, createdUser.Email);
   
    
    Assert.Equal(2, createdLists.Count);
    var likedList = createdLists.FirstOrDefault(l => l.Name == "Liked Movies");
    Assert.NotNull(likedList);
    Assert.False(likedList.Public);
    Assert.Equal(createdUser.Id, likedList.CreatorId);
    Assert.Equal(createdUser, likedList.Creator);

    var watchedList = createdLists.FirstOrDefault(l => l.Name == "Watched Movies");
    Assert.NotNull(watchedList);
    Assert.False(watchedList.Public);
    Assert.Equal(createdUser.Id, watchedList.CreatorId);
    Assert.Equal(createdUser, watchedList.Creator);

}

    
    
}