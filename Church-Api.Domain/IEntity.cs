using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church_Api.Data.Interfaces
{
    public interface IEntity
    {
        public string Id { get; set; }
        public string Type { get; }
        public DateTime Created { get; }
        public string CreatedBy { get; }
        public DateTime LastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
