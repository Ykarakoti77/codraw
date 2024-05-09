using System.Text.Json;
using Microsoft.Azure.Cosmos;

namespace Codraw.Framework.DbCore;

public class CosmosDbService<Item>(CosmosClient dbClient, string databaseName, string containerName)
    : ICosmosDbService<Item>
    where Item : ICosmosDbRecord
{
    private readonly Container _container = dbClient.GetContainer(databaseName, containerName);

    private readonly HashSet<Type> _acceptedKeyTypes =
    [
        typeof(string),
        typeof(bool),
        typeof(Double)
    ];

    private static dynamic Cast(dynamic obj, Type castTo)
    {
        return Convert.ChangeType(obj, castTo);
    }

    private Type GetType(object o)
    {
        if (_acceptedKeyTypes.Contains(o.GetType()))
        {
            return o.GetType();
        }

        throw new Exception("Unsupported...");
    }

    public async Task UpsertItemAsync(Item item)
    {
        var t = GetType(item.GetCosmosDbPartitionKey());
        Console.WriteLine(JsonSerializer.Serialize(item));
        await this._container.UpsertItemAsync(
            item,
            new PartitionKey(Cast(item.GetCosmosDbPartitionKey(), t))
        );
    }

    public async Task<IEnumerable<Item>> GetItemsAsync(
        string queryString,
        int maxItems = int.MaxValue
    )
    {
        var query = this._container.GetItemQueryIterator<Item>(new QueryDefinition(queryString));
        List<Item> results = [];
        int itemCount = 0;
        while (query.HasMoreResults && itemCount < maxItems)
        {
            var response = await query.ReadNextAsync();
            int itemsToAdd = Math.Min(response.Count, maxItems - itemCount);
            results.AddRange(response.Take(itemsToAdd));
            itemCount += itemsToAdd;
        }

        query.Dispose();

        return results;
    }

    public async Task ReplaceItemAsync(string id, Item item)
    {
        var t = GetType(item.GetCosmosDbPartitionKey());
        await this._container.ReplaceItemAsync<Item>(
            item,
            id,
            new PartitionKey(Cast(item.GetCosmosDbPartitionKey(), t))
        );
    }
}
