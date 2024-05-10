using System.Text.Json.Serialization;

namespace Codraw.Models.Auth;

public class LoginReponse
{
    [JsonPropertyName("bearerToken")]
    public string BearerToken { get; set; } = "";
}
