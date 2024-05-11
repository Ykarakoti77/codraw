using System.Security.Claims;
using Codraw.Models.Auth;
using Codraw.Models.User;

namespace Codraw.Services.AuthService;

public interface IAuthClient
{
    Task<string> AuthenticateAsync(PasswordLogin loginDetails);
    Task<string> JwtSignInAsync(List<Claim> claims);
    Task<bool> CheckUserInDbAsync(string email);
    Task<UserDetails> GetUserWithEmailAsync(string email);
}
