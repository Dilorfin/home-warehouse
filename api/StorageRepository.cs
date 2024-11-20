using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
 
namespace api;

public class StorageRepository
{
    private static readonly string ConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

    private const string DatabaseId = "home-warehouse";
    private const string ContainerId = "items";
    private const string PartitionKey = "/id";
    
    // The Cosmos client instance
    private readonly CosmosClient _cosmosClient = new(ConnectionString);

    // The database we will create
    private Database _database;

    // The container we will create.
    private Container _container;

    public async Task<bool> ContainsAsync(string id)
    {
        var queryable = _container.GetItemLinqQueryable<StorageModel>();
        var matches = queryable.Where(e => e.Id == id);
        using FeedIterator<StorageModel> linqFeed = matches.ToFeedIterator();
        while (linqFeed.HasMoreResults)
        {
            var response = await linqFeed.ReadNextAsync();
            return response.Count > 0;
        }
        return false;
    }

    public async Task<StorageModel> UpsertAsync(StorageModel item)
    {
        await Initialize();

        return await _container.UpsertItemAsync(item);
    }
    public async Task<StorageModel> GetAsync(string id)
    {
        await Initialize();

        try
        {
            return await _container.ReadItemAsync<StorageModel>(id, new PartitionKey(id));
        }
        catch (Exception)
        {
            return null;
        }
    }
    public async Task<StorageModel> DeleteAsync(StorageModel item)
    {
        await Initialize();
        
        return await _container.DeleteItemAsync<StorageModel>(item.Id, new PartitionKey(item.Id));
    }

    private async Task Initialize()
    {
        _database ??= await _cosmosClient.CreateDatabaseIfNotExistsAsync(DatabaseId);
        _container ??= await _database.CreateContainerIfNotExistsAsync(ContainerId, PartitionKey);
    }
}
