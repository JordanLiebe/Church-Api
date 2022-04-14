using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church_Api.Data.Models
{
    public class EntityStoreError
    {
        public string? Code { get; set; }
        public string? Message { get; set; }
        public string? EntityId { get; set; }
        public string? EntityType { get; set; }
    }
}
