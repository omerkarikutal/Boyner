using Core.Entity;
using Core.Model;
using Core.Repository;
using Dal.Abstract;
using DataAccess.Concrete.EF.Context;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Concrete
{
    public class ProductAttributeRepository : EfRepository<ProductAttribute>, IProductAttributeRepository
    {
        private BoynerContext _ctx;
        public ProductAttributeRepository(BoynerContext context) : base(context)
        {
            _ctx = context;
        }
        public async Task<BaseResponse<int>> UpdateList(int productId, List<int> attr)
        {
            //bu product ve attributeler ile eşleşen aktif kayıtları getir
            var productAttributes = await _ctx.ProductAttribute.Where(s => s.ProductId == productId && s.IsActive).Include(x => x.AttributeValue).ToListAsync();

            foreach (var item in attr)
            {
                var getAttribute = await _ctx.AttributeValue.FirstOrDefaultAsync(s => s.Id == item);


                var productAttribute = productAttributes.FirstOrDefault(s => s.AttributeValue.AttributeId == getAttribute.AttributeId);

                if (productAttribute != null)
                {
                    productAttribute.AttributeValueId = item;
                    _ctx.ProductAttribute.Update(productAttribute);
                }
                else
                {
                    await _ctx.ProductAttribute.AddAsync(new ProductAttribute { AttributeValueId = item, ProductId = productId });
                }
            }
            await _ctx.SaveChangesAsync();
            return new BaseResponse<int>().Success(1);
        }
    }
}
