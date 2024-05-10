using System.Security.Claims;
using Codraw.Models.Auth;

namespace Codraw.Services.AuthService;

public interface IAuthClient
{
    Task<string> AuthenticateAsync(PasswordLogin loginDetails);
    Task<string> JwtSignInAsync(List<Claim> claims);
}