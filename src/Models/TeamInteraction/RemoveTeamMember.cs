using System.Text.Json.Serialization;

namespace Codraw.Models.TeamInteraction;

public class RemoveTeamMember
{
    [JsonPropertyName("teamId")]
    public string TeamId { get; set; } = "";

    [JsonPropertyName("userId")]
    public string UserId { get; set; } = "";

    [JsonPropertyName("removerId")]
    public string RemoverId { get; set; } = "";
}