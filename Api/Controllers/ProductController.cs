using Bll.Cqrs.Commands.Product.Add;
using Bll.Cqrs.Commands.Product.Delete;
using Bll.Cqrs.Commands.Product.Edit;
using Bll.Cqrs.Queries.Product.GetAll;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllProductRequest query)
        {
            return Ok(await _mediator.Send(query));
        }
        [HttpPost]
        public async Task<IActionResult> Post(AddProductRequest query)
        {
            if (ModelState.IsValid)
                return Ok(await _mediator.Send(query));

            return BadRequest($"Wrong Model");
        }
        [HttpPut]
        public async Task<IActionResult> Put(EditProductRequest query)
        {
            if (ModelState.IsValid)
                return Ok(await _mediator.Send(query));

            return BadRequest($"Wrong Model");
        }
        [HttpDelete]
        public async Task<IActionResult> Put(int id)
        {
            return Ok(await _mediator.Send(new DeleteProductRequest { Id = id }));
        }
    }
}
