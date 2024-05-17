using System.Text.Json.Serialization;

namespace Codraw.Models.TeamInteraction;

public class CreateBoard
{
    [JsonPropertyName("boardName")]
    public string BoardName { get; set; } = "";

    [JsonPropertyName("teamId")]
    public string TeamId { get; set; } = "";
}