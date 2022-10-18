using Core.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.Cqrs.Commands.ProductCategory.Delete
{
    public class DeleteProductCategoryRequest:IRequest<BaseResponse<int>>
    {
        public int Id { get; set; }
    }
}
