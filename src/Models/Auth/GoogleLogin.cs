using System.Text.Json.Serialization;

namespace Codraw.Models.Auth;

public class GoogleLogin
{
    [JsonPropertyName("authorizationCode")]
    public string AuthorizationCode { get; set; } = "";
}
