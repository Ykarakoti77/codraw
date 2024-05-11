using System.Text.Json;
using Codraw.Configurations;
using Codraw.Models.Auth;
using Codraw.Services.AuthService;
using Codraw.Services.RegistrationServices;
using Codraw.Utils;

namespace Codraw.Services.GoogleAuthService;

public class GoogleAuthClient(
    ILogger<GoogleAuthClient> _logger,
    IConfigManager _configManager,
    IAuthClient _authClient,
    IRegistrationClient _registrationClient,
    HttpClient _httpClient
) : IGoogleAuthClient
{
    public async Task<string> GoogleSignInAsync(GoogleLogin loginDetails)
    {
        try
        {
            var configs = await _configManager.GetConfigurations();

            var content = await CreateUrlEncodedContent(loginDetails);
            var result = await _httpClient.PostAsync(
                configs.GoogleAuth.GoogleFetchAccessTokenUri,
                content
            );
            var responseString = await result.Content.ReadAsStringAsync();
            var googleAccessTokenResponse = JsonSerializer.Deserialize<GoogleAccessTokenResponse>(
                responseString
            );
            var tokenUserDetails =
                googleAccessTokenResponse?.GetUserDetails()
                ?? throw new Exception("google access token Response is null");

            if (await _authClient.CheckUserInDbAsync(tokenUserDetails.Email))
            {
                var userDetails = await _authClient.GetUserWithEmailAsync(tokenUserDetails.Email);
                return await _authClient.AuthenticateAsync(
                    new PasswordLogin()
                    {
                        Email = tokenUserDetails.Email,
                        Password = tokenUserDetails.Password
                    }
                );
            }

            await _registrationClient.RegisterUserAsync(tokenUserDetails);
            return await _authClient.AuthenticateAsync(
                new PasswordLogin()
                {
                    Email = tokenUserDetails.Email,
                    Password = tokenUserDetails.Password
                }
            );
        }
        catch (Exception ex)
        {
            _logger.LogInformation("Error in google sing in : {error}", ex.Message);
            throw new Exception("Error in google sing in.");
        }
    }

    private async Task<FormUrlEncodedContent> CreateUrlEncodedContent(GoogleLogin loginDetails)
    {
        var configs = await _configManager.GetConfigurations();

        return new FormUrlEncodedContent(
            [
                new KeyValuePair<string, string>("code", loginDetails.AuthorizationCode),
                new KeyValuePair<string, string>(
                    "client_id",
                    ConfigurationHelper.GetGoogleClientId()
                ),
                new KeyValuePair<string, string>(
                    "client_secret",
                    ConfigurationHelper.GetGoogleClientSecret()
                ),
                new KeyValuePair<string, string>("redirect_uri", configs.GoogleAuth.RedirectUri),
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
            ]
        );
    }
}
