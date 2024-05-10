using Codraw.Framework.DbCore;
using Codraw.Models.User;
using Codraw.Services.AuthService;

namespace Codraw.Services.RegistrationServices;

public class RegistrationClient(
    ILogger<RegistrationClient> _logger,
    ICosmosDbService<UserDetails> _userDbService,
    IAuthClient _authClient
) : IRegistrationClient
{
    public async Task RegisterUserAsync(UserDetails userDetails)
    {
        try
        {
            if (await _authClient.CheckUserInDbAsync(userDetails.Email))
                throw new Exception("User already registered");
            await _userDbService.UpsertItemAsync(userDetails);
        }
        catch (Exception ex)
        {
            _logger.LogInformation("error in registering user : {error}", ex.Message);
            throw;
        }
    }
}
