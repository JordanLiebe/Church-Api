using Church_Api.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church_Api.Data.Models
{
    public class EntityStoreResponse<T>
    {
        public EntityStoreResponse(T? data, int? count = null, IEnumerable<EntityStoreError>? errors = null)
        {
            Data = data;
            Count = count ?? (Data != null ? 1 : 0);
            Errors = errors ?? new List<EntityStoreError>();
        }

        public int Count { get; protected set; }
        public T? Data { get; protected set; }
        public IEnumerable<EntityStoreError> Errors { get; protected set; }
        public bool wasSuccessful
        {
            get
            {
                return Errors.Count() == 0 
                    && Data != null;
            }
        }
    }
}
