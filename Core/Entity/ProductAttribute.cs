using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity
{
    public class ProductAttribute : BaseEntity
    {


        public int ProductId { get; set; }
        public virtual Product Product { get; set; }



        public int AttributeValueId { get; set; }
        public virtual AttributeValue AttributeValue { get; set; }
    }
}
