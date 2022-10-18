using Core.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.Cqrs.Commands.ProductCategory.Edit
{
    public class EditProductCategoryRequest:IRequest<BaseResponse<int>>
    {
        public EditProductCategoryRequest()
        {
            Attributes = new List<int>();
        }
        public List<int> Attributes { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
