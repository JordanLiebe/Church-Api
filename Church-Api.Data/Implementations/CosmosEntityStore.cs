using Church_Api.Data.Interfaces;
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
        public CosmosEntityStore(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public T Create<T>(T item)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> CreateMany<T>(IEnumerable<T> items)
        {
            throw new NotImplementedException();
        }

        public T Delete<T>(string id)
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string id)
        {
            throw new NotImplementedException();
        }

        public T GetMany<T>(string query)
        {
            throw new NotImplementedException();
        }

        public T Update<T>(T item)
        {
            throw new NotImplementedException();
        }
    }
}
