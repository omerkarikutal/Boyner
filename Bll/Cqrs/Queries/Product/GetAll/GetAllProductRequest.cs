using Core.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.Cqrs.Queries.Product.GetAll
{
    public class GetAllProductRequest : IRequest<BaseResponse<List<Core.Entity.Product>>>
    {
        public string? Name { get; set; }
        public string? CategoryName { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }

    }
}
