using Core.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.Cqrs.Commands.Product.Delete
{
    public class DeleteProductRequest : IRequest<BaseResponse<int>>
    {
        [Required]
        public int Id { get; set; }
    }
}
