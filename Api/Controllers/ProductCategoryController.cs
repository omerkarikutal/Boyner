using Bll.Cqrs.Commands.ProductCategory.Add;
using Bll.Cqrs.Commands.ProductCategory.Delete;
using Bll.Cqrs.Commands.ProductCategory.Edit;
using Bll.Cqrs.Queries.ProductCategory.GetAllProductCategory;
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
    public class ProductCategoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductCategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllProductCategoryRequest query)
        {
            return Ok(await _mediator.Send(query));
        }
        [HttpPost]
        public async Task<IActionResult> Post(AddProductCategoryRequest query)
        {
            if (ModelState.IsValid)
                return Ok(await _mediator.Send(query));

            return BadRequest($"Wrong Model");
        }
        [HttpPut]
        public async Task<IActionResult> Put(EditProductCategoryRequest query)
        {
            if (ModelState.IsValid)
                return Ok(await _mediator.Send(query));

            return BadRequest($"Wrong Model");
        }
        [HttpDelete]
        public async Task<IActionResult> Put(int id)
        {
            return Ok(await _mediator.Send(new DeleteProductCategoryRequest { Id = id }));
        }
    }
}
