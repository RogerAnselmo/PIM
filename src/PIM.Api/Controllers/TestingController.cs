using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using PIM.Api.Core.Models;
using PIM.Api.Core.Services;
using PIM.Api.TransferObjects.Requests;

namespace PIM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestingController : ControllerBase
    {
        private readonly ProductService _productService;

        public TestingController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpPost(nameof(CreateProduct))]
        public async Task<ObjectResult> CreateProduct([FromBody] Product p)
        {
            const int max = 500;
            int index = 1;

            var random = new Random();

            while (index <= max)
            {
                var product = new ProductRequestModel
                {
                    Brand = $"Brand {random.Next(1, 5).ToString()}",
                    Category = $"Category {random.Next(6, 10).ToString()}",
                    Color = $"Color {random.Next(11, 15).ToString()}",
                    Name = $"Product {index}",
                    Price = (decimal)(random.NextDouble() * random.Next(10, 10000))
                };

                product.Description =
                    $"this is the {product.Name}. ${product.Color}. ${product.Brand}. ${product.Category}";

                await _productService.SaveAsync(product);
                index++;
            }

            return Created(nameof(CreateProduct), null);
        }

        [HttpGet(nameof(ApplicationIsAlive))]
        public ObjectResult ApplicationIsAlive() => Ok(new { message = "application is alive" });

        [HttpGet(nameof(AuthorizeIsWorking))]
        [Authorize]
        public ObjectResult AuthorizeIsWorking() => Ok(new { message = "authorize is working" });
    }
}
