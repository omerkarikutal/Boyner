using Core.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.Cqrs.Commands.ProductCategory.Add
{
    public class AddProductCategoryRequest : IRequest<BaseResponse<int>>
    {
        public AddProductCategoryRequest()
        {
            Attributes = new List<int>();
        }
        public List<int> Attributes { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
