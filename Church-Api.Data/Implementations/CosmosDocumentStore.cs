using Church_Api.Data.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church_Api.Data.Implementations
{
    public class CosmosDocumentStore : IDocumentStore
    {
        private readonly IConfiguration _configuration;
        public CosmosDocumentStore(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}
