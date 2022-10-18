using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity
{
    public class Attribute : BaseEntity
    {
        public string Name { get; set; }



        public virtual ICollection<AttributeValue> AttributeValues { get; set; }
    }
}
