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
    public interface IProductAttributeRepository : IEfRepository<ProductAttribute>
    {
        Task<BaseResponse<int>> UpdateList(int productId, List<int> attr);
    }
}
