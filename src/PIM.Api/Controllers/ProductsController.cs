using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PIM.Api.Core.Models;
using PIM.Api.Core.Services;
using PIM.Api.TransferObjects.Requests;

namespace PIM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService) => _productService = productService;

        [HttpPost(nameof(CreateProduct))]
        public async Task<ObjectResult> CreateProduct([FromBody] Product product)
        {

            const int max = 500;
            int index = 1;

            var random = new Random();

            while (index <= max)
            {
                product = new Product
                {
                    Brand = $"Brand {random.Next(1, 5).ToString()}",
                    Category = $"Category {random.Next(6, 10).ToString()}",
                    Color = $"Color {random.Next(11, 15).ToString()}",
                    Name = $"Product {index}",
                    Price = (decimal) (random.NextDouble() * random.Next(10,10000))
                };

                product.Description =
                    $"this is the {product.Name}. ${product.Color}. ${product.Brand}. ${product.Category}";

                await _productService.SaveAsync(product);
                index++;
            }



            //var result = await _productService.SaveAsync(product);
            //if (!result.Success)
            //    return BadRequest(result);

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
