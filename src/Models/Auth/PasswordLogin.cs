using System.Text.Json.Serialization;

namespace Codraw.Models.Auth;

public class PasswordLogin
{
    [JsonPropertyName("email")]
    public string Email { get; set; } = "";

    [JsonPropertyName("password")]
    public string Password { get; set; } = "";
}
