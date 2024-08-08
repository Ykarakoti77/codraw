using System.Text.Json.Serialization;

namespace Codraw.Models.Team;

public class TeamBoardUpdate
{
    [JsonPropertyName("teamBoardId")]
    public string TeamBoardId { get; set; } = "";

    [JsonPropertyName("teamBoardName")]
    public string TeamBoardName { get; set; } = "";

    [JsonPropertyName("data")]
    public List<TeamBoardData> Data { get; set; } = [];

    [JsonPropertyName("isDeleted")]
    public bool IsDeleted { get; set; } = false;
}

