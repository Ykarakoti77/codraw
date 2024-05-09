using System.Text.Json.Serialization;

namespace Codraw.Configurations;

public class CodrawConfiguration
{
    [JsonPropertyName("appVersion")]
    public string AppVersion { get; set; } = "";

    [JsonPropertyName("jwtTokenExpiryTimeInHours")]
    public int JwtTokenExpiryTimeInHours { get; set; }
}
