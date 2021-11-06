using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PIM.Api.Core.Models;
using PIM.Api.Core.Services;
using PIM.Api.TransferObjects.Requests;

namespace PIM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService) => _productService = productService;

        [HttpPost(nameof(CreateProduct))]
        public async Task<ObjectResult> CreateProduct([FromBody] Product product)
        {
            var result = await _productService.SaveAsync(product);
            if (!result.Success)
                return BadRequest(result);

            return Created(nameof(CreateProduct), null);
        }

        [HttpPut(nameof(UpdateProduct))]
        public async Task<ObjectResult> UpdateProduct(UpdateProduct product)
        {
            var result = await _productService.UpdateAsync(product);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet(nameof(GetByFilter))]
        public ObjectResult GetByFilter([FromQuery] ProductsFilterModel filter)
        {
            var products = _productService.GetByFilter(filter);
            return Ok(products);
        }
    }
}
