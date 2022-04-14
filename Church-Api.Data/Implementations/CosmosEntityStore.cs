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

        #region CreateMethods
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

                    EntityStoreResponse<T> storeResponse = new EntityStoreResponse<T>(createResults.Resource);

                    return storeResponse;
                }
                catch (CosmosException ex)
                {
                    List<EntityStoreError> errors = new List<EntityStoreError>();

                    EntityStoreError exError = new EntityStoreError()
                    {
                        Code = ex.StatusCode.ToString(),
                        Message = ex.Message,
                        EntityId = item.Id,
                        EntityType = item.Type,
                    };
                    errors.Add(exError);

                    EntityStoreResponse<T> resource = new EntityStoreResponse<T>(default, errors: errors);

                    return resource;
                }
            }
        }

        public async Task<EntityStoreResponse<IEnumerable<T>>> CreateManyAsync<T>(string containerId, IEnumerable<T> items) where T : IEntity
        {
            Queue<T> queue = new Queue<T>(items);

            List<T> dataList = new List<T>();
            List<EntityStoreError> errorList = new List<EntityStoreError>();

            while(queue.Count > 0)
            {
                var item = queue.Dequeue();

                var response = await CreateAsync(containerId, item);

                if(response.wasSuccessful)
                {
                    if (response.Data != null)
                    {
                        dataList.Add(response.Data);
                    }
                }
                else
                {
                    errorList.AddRange(response.Errors);
                }
            }

            return new EntityStoreResponse<IEnumerable<T>>(dataList, dataList.Count(), errorList);
        }
        #endregion

        #region ReadMethods
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

                    EntityStoreResponse<T> storeResponse = new EntityStoreResponse<T>(readResults.Resource);

                    return storeResponse;
                }
                catch (CosmosException ex)
                {
                    List<EntityStoreError> errors = new List<EntityStoreError>();

                    EntityStoreError exError = new EntityStoreError()
                    {
                        Code = ex.StatusCode.ToString(),
                        Message = ex.Message,
                        EntityId = id,
                        EntityType = genericType,
                    };
                    errors.Add(exError);

                    EntityStoreResponse<T> resource = new EntityStoreResponse<T>(default, errors: errors);

                    return resource;
                }
            }
        }

        public async Task<EntityStoreResponse<IEnumerable<T>>> GetManyAsync<T>(string containerId, string? queryString) where T : IEntity
        {
            string genericType = typeof(T).Name.ToLower();

            string whereClause = queryString != null ? " AND " + queryString : "";

            using (var cosmosClient = new CosmosClient(_cosmosConfig.EndpointUri, _cosmosConfig.PrimaryKey, _cosmosClientOptions))
            {
                try
                {
                    Database database = cosmosClient.GetDatabase(_cosmosConfig.DatabaseId);

                    Container container = await database.CreateContainerIfNotExistsAsync(containerId, "/type");

                    string cosmosQuery = $"SELECT * FROM store WHERE store.type = '{genericType}'" + whereClause;

                    QueryDefinition queryDefinition = new QueryDefinition(cosmosQuery);

                    FeedIterator<T> queryResultSetIterator = container.GetItemQueryIterator<T>(queryDefinition);

                    List<T> itemList = new List<T>();

                    while (queryResultSetIterator.HasMoreResults)
                    {
                        FeedResponse<T> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                        foreach (T result in currentResultSet)
                        {
                            itemList.Add(result);
                        }
                    }

                    EntityStoreResponse<IEnumerable<T>> storeResponse = new EntityStoreResponse<IEnumerable<T>>(itemList, count: itemList.Count);

                    return storeResponse;
                }
                catch (CosmosException ex)
                {
                    List<EntityStoreError> errors = new List<EntityStoreError>();

                    EntityStoreError exError = new EntityStoreError()
                    {
                        Code = ex.StatusCode.ToString(),
                        Message = ex.Message,
                        EntityType = genericType,
                    };
                    errors.Add(exError);

                    EntityStoreResponse<IEnumerable<T>> resource = new EntityStoreResponse<IEnumerable<T>>(default, errors: errors);

                    return resource;
                }
            }
        }
        #endregion

        #region UpdateMethods
        public async Task<EntityStoreResponse<T>> UpdateAsync<T>(string containerId, T item) where T : IEntity
        {
            string genericType = typeof(T).Name.ToLower();

            using (var cosmosClient = new CosmosClient(_cosmosConfig.EndpointUri, _cosmosConfig.PrimaryKey, _cosmosClientOptions))
            {
                try
                {
                    Database database = cosmosClient.GetDatabase(_cosmosConfig.DatabaseId);

                    Container container = await database.CreateContainerIfNotExistsAsync(containerId, "/type");

                    ItemResponse<T> readResults = await container.ReadItemAsync<T>(item.Id, new PartitionKey(genericType));

                    if (readResults.Resource.Type != genericType)
                    {
                        throw new Exception("Cannot change the type of an existing record.");
                    }

                    var updateResults = await container.UpsertItemAsync(item, new PartitionKey(genericType));

                    EntityStoreResponse<T> storeResponse = new EntityStoreResponse<T>(updateResults.Resource);

                    return storeResponse;
                }
                catch (CosmosException ex)
                {
                    List<EntityStoreError> errors = new List<EntityStoreError>();

                    EntityStoreError exError = new EntityStoreError()
                    {
                        Code = ex.StatusCode.ToString(),
                        Message = ex.Message,
                        EntityId = item.Id,
                        EntityType = item.Type,
                    };
                    errors.Add(exError);

                    EntityStoreResponse<T> resource = new EntityStoreResponse<T>(default, errors: errors);

                    return resource;
                }
            }
        }

        public async Task<EntityStoreResponse<IEnumerable<T>>> UpdateManyAsync<T>(string containerId, IEnumerable<T> items) where T : IEntity
        {
            Queue<T> queue = new Queue<T>(items);

            List<T> dataList = new List<T>();
            List<EntityStoreError> errorList = new List<EntityStoreError>();

            while (queue.Count > 0)
            {
                var item = queue.Dequeue();

                var response = await UpdateAsync(containerId, item);

                if (response.wasSuccessful)
                {
                    if (response.Data != null)
                    {
                        dataList.Add(response.Data);
                    }
                }
                else
                {
                    errorList.AddRange(response.Errors);
                }
            }

            return new EntityStoreResponse<IEnumerable<T>>(dataList, dataList.Count(), errorList);
        }
        #endregion

        #region DeleteMethods
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

                    EntityStoreResponse<T> storeResponse = new EntityStoreResponse<T>(deleteResults.Resource);

                    return storeResponse;
                }
                catch (CosmosException ex)
                {
                    List<EntityStoreError> errors = new List<EntityStoreError>();

                    EntityStoreError exError = new EntityStoreError()
                    {
                        Code = ex.StatusCode.ToString(),
                        Message = ex.Message,
                        EntityId = id,
                        EntityType = genericType,
                    };
                    errors.Add(exError);

                    EntityStoreResponse<T> resource = new EntityStoreResponse<T>(default, errors: errors);

                    return resource;
                }
            }
        }

        public async Task<EntityStoreResponse<IEnumerable<T>>> DeleteManyAsync<T>(string containerId, IEnumerable<string> ids) where T : IEntity
        {
            Queue<string> queue = new Queue<string>(ids);

            List<T> dataList = new List<T>();
            List<EntityStoreError> errorList = new List<EntityStoreError>();

            while (queue.Count > 0)
            {
                var id = queue.Dequeue();

                if(string.IsNullOrEmpty(id))
                    continue;

                var response = await DeleteAsync<T>(containerId, id);

                if (response.wasSuccessful)
                {
                    if (response.Data != null)
                    {
                        dataList.Add(response.Data);
                    }
                }
                else
                {
                    errorList.AddRange(response.Errors);
                }
            }

            return new EntityStoreResponse<IEnumerable<T>>(dataList, dataList.Count(), errorList);
        }
        #endregion
    }
}
