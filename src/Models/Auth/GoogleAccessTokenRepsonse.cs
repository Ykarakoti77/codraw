using System.IdentityModel.Tokens.Jwt;
using System.Text.Json.Serialization;
using Codraw.Models.User;

namespace Codraw.Models.Auth;

public class GoogleAccessTokenResponse
{
    private static readonly JwtSecurityTokenHandler handler = new();

    [JsonPropertyName("access_token")]
    public string? AccessToken { get; set; }

    [JsonPropertyName("expires_in")]
    public int? ExpireTime { get; set; }

    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; set; }

    [JsonPropertyName("scope")]
    public string? Scope { get; set; }

    [JsonPropertyName("token_type")]
    public string? TokenType { get; set; }

    [JsonPropertyName("id_token")]
    public string? IdToken { get; set; }

    [JsonPropertyName("error")]
    public string? Error { get; set; }

    [JsonPropertyName("error_description")]
    public string? ErrorDescription { get; set; }
    
    public UserDetails GetUserDetails()
    {
        var jwtSecurityToken = handler.ReadJwtToken(this.IdToken);
        var email = jwtSecurityToken.Claims.First(claim => claim.Type == "email").Value;
        var first_name = jwtSecurityToken.Claims.First(claim => claim.Type == "given_name").Value;
        var last_name = jwtSecurityToken.Claims.First(claim => claim.Type == "family_name").Value;

        return new UserDetails() {
            Email = email,
            FirstName = first_name,
            LastName = last_name,
            Password = Guid.NewGuid().ToString()
        };
    }
}

public class GoogleAccessTokenErrorResponse
{
    [JsonPropertyName("code")]
    public int Code { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    public override string ToString()
    {
        return $"GoogleAccessTokenError - Code: {this.Code}, Message: {this.Message}, Status: {this.Status}";
    }
}