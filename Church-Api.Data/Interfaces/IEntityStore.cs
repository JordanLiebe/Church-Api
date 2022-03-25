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
        public T Create<T>(T item);

        /// <summary>
        /// Creates multiple items in the Entity Store.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public IEnumerable<T> CreateMany<T>(IEnumerable<T> items);

        /// <summary>
        /// Reads one item from the Entity Store using its unique id.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Get<T>(string id);

        /// <summary>
        /// Reads many items from the Entity Store using a Cosmos Query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public T GetMany<T>(string query);

        /// <summary>
        /// Updates an item in the Entity Store using its id a replacement object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public T Update<T>(T item);

        /// <summary>
        /// Deletes an item from the Entity Store by its unique id.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Delete<T>(string id);
    }
}
