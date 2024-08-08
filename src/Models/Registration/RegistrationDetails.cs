using System.Text.Json.Serialization;

namespace Codraw.Models.Registration;

public class RegistrationDetails
{
    [JsonPropertyName("email")]
    public string Email { get; set; } = "";

    [JsonPropertyName("firstName")]
    public string FirstName { get; set; } = "";

    [JsonPropertyName("lastName")]
    public string LastName { get; set; } = "";

    [JsonPropertyName("userName")]
    public string UserName { get; set; } = "";

    [JsonPropertyName("password")]
    public string Password { get; set; } = "";
}
