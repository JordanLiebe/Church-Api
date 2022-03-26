using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church_Api.Data.Models
{
    public class CosmosConfiguration
    {
        public string? EndpointUri { get; set; }
        public string? PrimaryKey { get; set; }
        public string? ApplicationName { get; set; }
        public string? DatabaseId { get; set; }
    }
}
