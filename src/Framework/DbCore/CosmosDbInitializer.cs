using System.Text.Json;
using Microsoft.Azure.Cosmos;

namespace Codraw.Framework.DbCore;

public static class CosmosDbInitializer
{
    public static CosmosDbService<T> InitializeCosmosClientInstance<T>(
        IConfigurationSection configurationSection
    )
        where T : ICosmosDbRecord
    {   
        Console.WriteLine(JsonSerializer.Serialize(configurationSection));
        string databaseName =
            configurationSection["DatabaseName"]
            ?? throw new ArgumentException("DatabaseName cannot be empty");
        string containerName =
            configurationSection["ContainerName"]
            ?? throw new ArgumentException("ContainerName cannot be empty");
        string account =
            configurationSection["Account"]
            ?? throw new ArgumentException("Account cannot be empty");
        string key =
            configurationSection["Key"] ?? throw new ArgumentException("Key cannot be empty");

        CosmosClient client =
            new(
                account,
                key,
                new CosmosClientOptions
                {
                    SerializerOptions = new CosmosSerializationOptions
                    {
                        PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                    }
                }
            );

        return new CosmosDbService<T>(client, databaseName, containerName);
    }
}
