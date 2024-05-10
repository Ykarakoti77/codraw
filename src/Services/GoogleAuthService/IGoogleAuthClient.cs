using Codraw.Models.Auth;

namespace Codraw.Services.GoogleAuthService;

public interface IGoogleAuthClient
{
    Task<string> GoogleSignInAsync(GoogleLogin loginDetails);
}