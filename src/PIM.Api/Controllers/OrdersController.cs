using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using PIM.Api.Core.Services;
using PIM.Api.TransferObjects.Requests;

namespace PIM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet(nameof(GetAllOrders))]
        public ObjectResult GetAllOrders()
        {
            return Ok(_orderService.GetAll());
        }

        [HttpPost(nameof(SaveOrder))]
        public async Task<ObjectResult> SaveOrder(NewOrderRequest newOrderRequest)
        {
            var result = await _orderService.SaveAsync(newOrderRequest);

            if (!result.Success)
                return BadRequest(result.Message);

            return Created(nameof(SaveOrder), result);
        }
    }
}
