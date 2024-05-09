namespace Codraw.Framework.DbCore;

public interface ICosmosDbService<Item>
{
    Task<IEnumerable<Item>> GetItemsAsync(string query, int maxItems = int.MaxValue);
    Task UpsertItemAsync(Item item);
    Task ReplaceItemAsync(string id, Item item);
}
