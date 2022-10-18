using Core.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.Cqrs.Queries.ProductCategory.GetAllProductCategory
{
    public class GetAllProductCategoryRequest : IRequest<BaseResponse<List<Core.Entity.ProductCategory>>>
    {
        public string? Name { get; set; }
    }
}
