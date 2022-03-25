using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church_Api.Domain
{
    [DebuggerDisplay("{Type}/{Id}")]
    public class EntityBase : IEntity
    {
        public EntityBase(string? id = null)
        {
            if (id == null)
                id = Guid.NewGuid().ToString();

            Id = id;
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
    }
}
