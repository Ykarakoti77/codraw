using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Codraw.Configurations;
using Codraw.Framework.DbCore;
using Codraw.Models.Auth;
using Codraw.Models.User;
using Codraw.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Codraw.Services.AuthService;

public readonly record struct UserToGenerateJwt(string UserId, string FirstName, string LastName);

public class AuthClient(
    IConfigManager _configManager,
    ICosmosDbService<UserDetails> _userDbService,
    ILogger<AuthClient> _logger
) : IAuthClient
{
    private static readonly byte[] _tokenKey = Encoding.ASCII.GetBytes(
        ConfigurationHelper.GetJwtKey()
    );

    public async Task<string> AuthenticateAsync(PasswordLogin loginDetails)
    {
        var user = await TryGetUserAsync(loginDetails);
        var claims = new List<Claim>
        {
            new("UserId", user.UserId),
            new(ClaimTypes.GivenName, user.FirstName),
            new(ClaimTypes.Surname, user.LastName)
        };
        return await JwtSignInAsync(claims);
    }

    public async Task<string> JwtSignInAsync(List<Claim> claims)
    {
        try
        {
            var configs = await _configManager.GetConfigurations();
            var claimsIdentity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddHours(configs.JwtTokenExpiryTimeInHours),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(_tokenKey),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
        catch (Exception ex)
        {
            _logger.LogError("error in jwt sign in : {error}", ex.Message);
            throw new Exception("error in jwt sign in");
        }
    }

    private async Task<UserDetails> TryGetUserAsync(PasswordLogin loginDetails)
    {
        var query =
            $" SELECT * FROM c where c.email = '{loginDetails.Email}' and c.password = '{loginDetails.Password}' ";
        var result = await _userDbService.GetItemsAsync(query);
        if (result.Count() > 1)
            throw new Exception("Multiple users exist");
        if (result.IsNullOrEmpty())
            throw new Exception("User not found");

        var user = result.First();
        _logger.LogInformation("User found in db with userId {userId}", user.UserId);

        return user;
    }
}
