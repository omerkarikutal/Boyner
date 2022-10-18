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
    public class CategoryAttributeRepository : EfRepository<CategoryAttribute>, ICategoryAttributeRepository
    {
        private BoynerContext _ctx;
        public CategoryAttributeRepository(BoynerContext context) : base(context)
        {
            _ctx = context;
        }

        public async Task<BaseResponse<int>> UpdateAttributes(int ıd, List<int> attributes)
        {
            //bu category ve attributeler ile eşleşen aktif kayıtları getir
            var categoryAttributes = await _ctx.CategoryAttribute.Where(s => s.ProductCategoryId == ıd && s.IsActive).Include(x => x.AttributeValue).ToListAsync();

            if (attributes.Count > 0)
            {
                foreach (var item in attributes)
                {
                    var getAttribute = await _ctx.AttributeValue.FirstOrDefaultAsync(s => s.Id == item);


                    var categoryAttribute = categoryAttributes.FirstOrDefault(s => s.AttributeValue.AttributeId == getAttribute.AttributeId);

                    if (categoryAttribute != null)
                    {
                        categoryAttribute.AttributeValueId = item;
                        _ctx.CategoryAttribute.Update(categoryAttribute);
                    }
                    else
                    {
                        await _ctx.CategoryAttribute.AddAsync(new CategoryAttribute { AttributeValueId = item, ProductCategoryId = ıd });
                    }

                }
                await _ctx.SaveChangesAsync();
            }
            return new BaseResponse<int>().Success(1);
        }
    }
}
