using Core.Model;
using Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Abstract
{
    public interface ICategoryAttributeRepository : IEfRepository<Core.Entity.CategoryAttribute>
    {
        Task<BaseResponse<int>> UpdateAttributes(int ıd, List<int> attributes);
    }
}
