using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity
{
    public class AttributeValue : BaseEntity
    {
        public string Value { get; set; }



        //navigation properties
        public int AttributeId { get; set; }
        public virtual Attribute Attribute { get; set; }
    }
}
