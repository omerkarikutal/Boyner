using Core.Entity;
using Core.Model;
using Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Abstract
{
    public interface IProductCategoryRepository : IEfRepository<ProductCategory>
    {
        BaseResponse<List<ProductCategory>> GetAll();
    }
}
