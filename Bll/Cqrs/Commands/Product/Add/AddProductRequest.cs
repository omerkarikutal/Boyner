using Core.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.Cqrs.Commands.Product.Add
{
    //product objesi dto modeline çevrilecek todo
    public class AddProductRequest : IRequest<BaseResponse<Core.Entity.Product>>
    {
        public AddProductRequest()
        {
            Attributes = new List<int>();
        }
        public List<int> Attributes { get; set; }
        public int ProductCategoryId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
    }
}
