using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public double Price { get; set; }


        public int ProductCategoryId { get; set; }
        //navigation properties
        public virtual ProductCategory ProductCategory { get; set; }




        public virtual ICollection<ProductAttribute> ProductAttributes { get; set; }
    }
}
