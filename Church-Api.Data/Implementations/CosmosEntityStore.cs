using Church_Api.Data.Interfaces;
using Church_Api.Data.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church_Api.Data.Implementations
{
    public class CosmosEntityStore : IEntityStore
    {
        private readonly IConfiguration _configuration;
        private readonly CosmosConfiguration _cosmosConfig;
        private readonly CosmosClientOptions _cosmosClientOptions;

        public CosmosEntityStore(IConfiguration configuration)
        {
            _configuration = configuration;
            _cosmosConfig = _configuration.GetSection("CosmosDb").Get<CosmosConfiguration>();
            _cosmosClientOptions = new CosmosClientOptions() { ApplicationName = _cosmosConfig.ApplicationName };

        }

        public async Task<EntityStoreResponse<T>> CreateAsync<T>(string containerId, T item) where T : IEntity
        {
            string genericType = typeof(T).Name.ToLower();

            using (var cosmosClient = new CosmosClient(_cosmosConfig.EndpointUri, _cosmosConfig.PrimaryKey, _cosmosClientOptions))
            {
                try
                {
                    Database database = cosmosClient.GetDatabase(_cosmosConfig.DatabaseId);

                    Container container = await database.CreateContainerIfNotExistsAsync(containerId, "/type");

                    var createResults = await container.CreateItemAsync(item, new PartitionKey(genericType));

                    EntityStoreResponse<T> storeResponse = new EntityStoreResponse<T>()
                    {
                        Document = createResults.Resource
                    };

                    return storeResponse;
                }
                catch (CosmosException ex)
                {
                    List<string> errors = new List<string>();
                    errors.Add(new string(ex.Message));

                    EntityStoreResponse<T> resource = new EntityStoreResponse<T>()
                    {
                        Document = default(T),
                        Errors = errors
                    };

                    return resource;
                }
            }
        }

        public Task<EntityStoreManyResponse<T>> CreateManyAsync<T>(IEnumerable<T> items) where T : IEntity
        {
            throw new NotImplementedException();
        }

        public async Task<EntityStoreResponse<T>> GetAsync<T>(string containerId, string id) where T : IEntity
        {
            string genericType = typeof(T).Name.ToLower();

            using (var cosmosClient = new CosmosClient(_cosmosConfig.EndpointUri, _cosmosConfig.PrimaryKey, _cosmosClientOptions))
            {
                try
                {
                    Database database = cosmosClient.GetDatabase(_cosmosConfig.DatabaseId);

                    Container container = await database.CreateContainerIfNotExistsAsync(containerId, "/type");

                    ItemResponse<T> readResults = await container.ReadItemAsync<T>(id, new PartitionKey(genericType));

                    EntityStoreResponse<T> storeResponse = new EntityStoreResponse<T>()
                    {
                        Document = readResults.Resource
                    };

                    return storeResponse;
                }
                catch (CosmosException ex)
                {
                    List<string> errors = new List<string>();
                    errors.Add(new string(ex.Message));

                    EntityStoreResponse<T> resource = new EntityStoreResponse<T>()
                    {
                        Document = default(T),
                        Errors = errors
                    };

                    return resource;
                }
            }
        }

        public Task<EntityStoreManyResponse<T>> GetManyAsync<T>(string query) where T : IEntity
        {
            throw new NotImplementedException();
        }

        public async Task<EntityStoreResponse<T>> UpdateAsync<T>(string containerId, string id, T item) where T : IEntity
        {
            string genericType = typeof(T).Name.ToLower();

            using (var cosmosClient = new CosmosClient(_cosmosConfig.EndpointUri, _cosmosConfig.PrimaryKey, _cosmosClientOptions))
            {
                try
                {
                    Database database = cosmosClient.GetDatabase(_cosmosConfig.DatabaseId);

                    Container container = await database.CreateContainerIfNotExistsAsync(containerId, "/type");

                    ItemResponse<T> readResults = await container.ReadItemAsync<T>(id, new PartitionKey(genericType));

                    if (readResults.Resource.Type != genericType)
                    {
                        throw new Exception("Cannot change the type of an existing record.");
                    }

                    var updateResults = await container.UpsertItemAsync(item, new PartitionKey(genericType));

                    EntityStoreResponse<T> storeResponse = new EntityStoreResponse<T>()
                    {
                        Document = updateResults.Resource
                    };

                    return storeResponse;
                }
                catch (CosmosException ex)
                {
                    List<string> errors = new List<string>();
                    errors.Add(new string(ex.Message));

                    EntityStoreResponse<T> resource = new EntityStoreResponse<T>()
                    {
                        Document = default(T),
                        Errors = errors
                    };

                    return resource;
                }
            }
        }

        public async Task<EntityStoreResponse<T>> DeleteAsync<T>(string containerId, string id) where T : IEntity
        {
            string genericType = typeof(T).Name.ToLower();

            using (var cosmosClient = new CosmosClient(_cosmosConfig.EndpointUri, _cosmosConfig.PrimaryKey, _cosmosClientOptions))
            {
                try
                {
                    Database database = cosmosClient.GetDatabase(_cosmosConfig.DatabaseId);

                    Container container = database.GetContainer(containerId);

                    var deleteResults = await container.DeleteItemAsync<T>(id, new PartitionKey(genericType));

                    EntityStoreResponse<T> storeResponse = new EntityStoreResponse<T>()
                    {
                        Document = deleteResults.Resource
                    };

                    return storeResponse;
                }
                catch (CosmosException ex)
                {
                    List<string> errors = new List<string>();
                    errors.Add(new string(ex.Message));

                    EntityStoreResponse<T> resource = new EntityStoreResponse<T>()
                    {
                        Document = default(T),
                        Errors = errors
                    };

                    return resource;
                }
            }
        }
    }
}
