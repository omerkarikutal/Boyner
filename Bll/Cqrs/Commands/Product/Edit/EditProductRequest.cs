using Core.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.Cqrs.Commands.Product.Edit
{
    public class EditProductRequest : IRequest<BaseResponse<Core.Entity.Product>>
    {
        public EditProductRequest()
        {
            Attributes = new List<int>();
        }
        [Required]
        public int Id { get; set; }
        public List<int> Attributes { get; set; }
        public int ProductCategoryId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
