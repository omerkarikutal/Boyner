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
    public class ProductRepository : EfRepository<Product>, IProductRepository
    {
        private BoynerContext _ctx;
        public ProductRepository(BoynerContext context) : base(context)
        {
            _ctx = context;
        }

        public BaseResponse<List<Product>> GetAll()
        {
            var result =   _ctx.Product.Where(s => s.IsActive).ToList();

            return new BaseResponse<List<Product>>().Success(result);
        }
    }
}
