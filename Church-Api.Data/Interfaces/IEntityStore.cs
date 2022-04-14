using Church_Api.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church_Api.Data.Interfaces
{
    public interface IEntityStore
    {
        /// <summary>
        /// Creates a new item in the Entity Store.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<EntityStoreResponse<T>> CreateAsync<T>(string containerId, T item) where T : IEntity;

        /// <summary>
        /// Creates multiple items in the Entity Store.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public Task<EntityStoreResponse<IEnumerable<T>>> CreateManyAsync<T>(string containerId, IEnumerable<T> items) where T : IEntity;

        /// <summary>
        /// Reads one item from the Entity Store using its unique id.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<EntityStoreResponse<T>> GetAsync<T>(string containerId, string id) where T : IEntity;

        /// <summary>
        /// Reads many items from the Entity Store using a Cosmos Query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public Task<EntityStoreResponse<IEnumerable<T>>> GetManyAsync<T>(string containerId, string? queryString) where T : IEntity;

        /// <summary>
        /// Updates an item in the Entity Store using its id a replacement object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<EntityStoreResponse<T>> UpdateAsync<T>(string containerId, T item) where T : IEntity;

        /// <summary>
        /// Updates an item in the Entity Store using its id a replacement object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<EntityStoreResponse<IEnumerable<T>>> UpdateManyAsync<T>(string containerId, IEnumerable<T> items) where T : IEntity;

        /// <summary>
        /// Deletes an item from the Entity Store by its unique id.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<EntityStoreResponse<T>> DeleteAsync<T>(string containerId, string id) where T : IEntity;

        /// <summary>
        /// Deletes an item from the Entity Store by its unique id.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<EntityStoreResponse<IEnumerable<T>>> DeleteManyAsync<T>(string containerId, IEnumerable<string> ids) where T : IEntity;
    }
}
