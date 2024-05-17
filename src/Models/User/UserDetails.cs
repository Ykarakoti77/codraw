using System.Text.Json.Serialization;
using Codraw.Framework.DbCore;

namespace Codraw.Models.User;

public class UserDetails() : ICosmosDbRecord
{
    // cosmos db id
    [JsonPropertyName("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [JsonPropertyName("userId")]
    public string UserId { get; set; } = Guid.NewGuid().ToString();

    [JsonPropertyName("partitionKey")]
    public string PartitionKey { get; set; } = DateTime.UtcNow.ToString("MM-yyyy");

    [JsonPropertyName("email")]
    public string Email { get; set; } = "";

    [JsonPropertyName("firstName")]
    public string FirstName { get; set; } = "";

    [JsonPropertyName("lastName")]
    public string LastName { get; set; } = "";

    [JsonPropertyName("userName")]
    public string UserName { get; set; } = "";

    [JsonPropertyName("password")]
    public string Password { get; set; } = "";

    public object GetCosmosDbPartitionKey()
    {
        return this.PartitionKey;
    }
}
