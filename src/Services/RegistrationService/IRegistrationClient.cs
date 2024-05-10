using Codraw.Models.User;

namespace Codraw.Services.RegistrationServices;

public interface IRegistrationClient
{
    Task RegisterUserAsync(UserDetails userDetails);
}