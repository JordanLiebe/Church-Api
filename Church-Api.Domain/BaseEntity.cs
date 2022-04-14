using Church_Api.Data.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church_Api.Data.Implementations
{
    [DebuggerDisplay("{Type}/{Id}")]
    public class BaseEntity : IEntity
    {
        public BaseEntity(string? id = null)
        {
            if (id == null)
                id = Guid.NewGuid().ToString();

            Id = id;
            Created = DateTime.Now;
            CreatedBy = "System";
            LastUpdated = DateTime.Now;
            LastUpdatedBy = "System";
        }

        [Key, JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [Key, JsonProperty(PropertyName = "type")]
        public string Type
        {
            get
            {
                return this.GetType().Name.ToLower();
            }
        }

        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
