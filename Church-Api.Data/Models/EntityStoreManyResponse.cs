using Church_Api.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church_Api.Data.Models
{
    public class EntityStoreManyResponse<T> where T : IEntity
    {
        public int Count { get; set; }
        public IEnumerable<T>? Documents { get; set; }
        public IEnumerable<string>? Errors { get; set; }
        public bool hasErrors
        {
            get
            {
                return Errors != null && Errors.Count() > 0;
            }
        }
        public bool succeeded
        {
            get
            {
                return Errors == null || Errors.Count() > 0;
            }
        }
    }
}
