using Church_Api.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church_Api.Domain
{
    public class JsonResource<T> where T : IEntity
    {
        public JsonResource()
        {

        }

        public JsonResource(T Entity)
        {
            this.Attributes = new Dictionary<string, string>();
            this.Relationships = new Dictionary<string, string>();
            this.Links = new Dictionary<string, string>();

            Type = Entity.Type;
            Id = Entity.Id;
            foreach(var property in typeof(T).GetProperties())
            {
                if(!property.PropertyType.IsAssignableFrom(typeof(IEntity)))
                {
                    string attributeName = property.Name;
                    string? attributeValue = property.GetValue(Entity)?.ToString();

                    Attributes.Add(attributeName, attributeValue ?? "");
                }
            }
        }

        public string Type { get; set; }
        public string Id { get; set; }
        public Dictionary<string, string> Attributes { get; set; }
        public Dictionary<string, string> Relationships { get; set; }
        public Dictionary<string, string> Links { get; set; }
    }
}
