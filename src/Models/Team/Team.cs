using System.Text.Json.Serialization;
using Codraw.Framework.DbCore;

namespace Codraw.Models.Team;

public class Team : ICosmosDbRecord
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [JsonPropertyName("partitionKey")]
    public string PartitionKey { get; set; } = DateTime.UtcNow.ToString("MM-yyyy");

    [JsonPropertyName("teamId")]
    public string TeamId { get; set; } = Guid.NewGuid().ToString();

    [JsonPropertyName("teamName")]
    public string TeamName { get; set; } = "";

    [JsonPropertyName("teamMembers")]
    public List<TeamMember> TeamMembers { get; set; } = [];
    

    [JsonPropertyName("teamBoards")]
    public List<string> TeamBoards { get; set; } = [];

    [JsonPropertyName("isDeleted")]
    public bool IsDeleted { get; set; } = false;

    [JsonPropertyName("teamInviteId")]
    public string TeamInviteId { get; set; } = Guid.NewGuid().ToString();

    public object GetCosmosDbPartitionKey()
    {
        return this.PartitionKey;
    }
}
