namespace Codraw.Framework.DbCore;

public interface ICosmosDbRecord
{
    public object GetCosmosDbPartitionKey();
}