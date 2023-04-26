using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using CineClubApi.Models.Auth;
using CineClubApi.Services.AccountService;
using Microsoft.IdentityModel.Tokens;

namespace CineClubApi.Services.TokenService;

public class TokenServiceImpl : ITokenService
{

    private readonly IConfiguration _configuration;

    public TokenServiceImpl( IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public string CreateToken(User user)
    {
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
            _configuration.GetSection("AppSettings:Token").Value));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            expires: DateTime.Now.AddDays(30),
            signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }

    public RefreshToken GenerateRefreshToken()
    {
        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Expires = DateTime.UtcNow.AddDays(30),
            Created = DateTime.UtcNow
        };

        return refreshToken;
    }
}