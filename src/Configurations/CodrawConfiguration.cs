using System.Text.Json.Serialization;

namespace Codraw.Configurations;

public class CodrawConfiguration
{
    [JsonPropertyName("appVersion")]
    public string AppVersion { get; set; } = "";

    [JsonPropertyName("jwtTokenExpiryTimeInHours")]
    public int JwtTokenExpiryTimeInHours { get; set; }

    [JsonPropertyName("googleAuth")]
    public GoogleAuth GoogleAuth { get; set; } = new GoogleAuth();
}

public class GoogleAuth 
{
    [JsonPropertyName("redirectUri")]
    public string RedirectUri { get; set; } = "";

    [JsonPropertyName("googleFetchAccessTokenUri")]
    public string GoogleFetchAccessTokenUri { get; set; } = "";
}