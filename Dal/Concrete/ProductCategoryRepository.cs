using Core.Entity;
using Core.Model;
using Core.Repository;
using Dal.Abstract;
using DataAccess.Concrete.EF.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Concrete.EF
{
    public class ProductCategoryRepository : EfRepository<ProductCategory>, IProductCategoryRepository
    {
        private BoynerContext _ctx;
        public ProductCategoryRepository(BoynerContext context) : base(context)
        {
            _ctx = context;
        }

        public BaseResponse<List<ProductCategory>> GetAll()
        {
            var result = _ctx.ProductCategory.Where(s => s.IsActive).ToList();

            return new BaseResponse<List<ProductCategory>>().Success(result);

        }
    }
}
