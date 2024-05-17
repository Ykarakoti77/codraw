using System.Text.Json.Serialization;
using Codraw.Framework.DbCore;

namespace Codraw.Models.Team;

public class TeamBoard : ICosmosDbRecord
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [JsonPropertyName("partitionKey")]
    public string PartitionKey { get; set; } = DateTime.UtcNow.ToString("MM-yyyy");

    [JsonPropertyName("teamBoardId")]
    public string TeamBoardId { get; set; } = Guid.NewGuid().ToString();

    [JsonPropertyName("teamBoardName")]
    public string TeamBoardName { get; set; } = "";

    [JsonPropertyName("data")]
    public List<TeamBoardData> Data { get; set; } = [];

    [JsonPropertyName("isDeleted")]
    public bool IsDeleted { get; set; } = false;

    public object GetCosmosDbPartitionKey()
    {
        return this.PartitionKey;
    }
}

public class TeamBoardData 
{
    [JsonPropertyName("jsonData")]
    public string JsonData { get; set; } = "";

    [JsonPropertyName("version")]
    public string Version { get; set; } = "";
}