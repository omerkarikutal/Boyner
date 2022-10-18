using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity
{
    public class CategoryAttribute : BaseEntity
    {
        public int ProductCategoryId { get; set; }
        public int AttributeValueId { get; set; }
        public virtual AttributeValue AttributeValue { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }

    }
}
