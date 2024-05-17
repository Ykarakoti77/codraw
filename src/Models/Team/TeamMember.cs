using System.Text.Json.Serialization;

namespace Codraw.Models.Team;

public class TeamMember
{
    [JsonPropertyName("userId")]
    public string UserId { get; set; } = "";

    [JsonPropertyName("role")]
    public Role Role { get; set; } = Role.MEMBER;
}

public enum Role
{
    ADMIN = 0, 
    MEMBER = 1, 
    MODERATOR = 2,
}