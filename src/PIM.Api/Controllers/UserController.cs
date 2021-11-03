using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using PIM.Api.Core.Services;
using PIM.Api.Models;

namespace PIM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly SystemUserService _userService;

        public UserController(SystemUserService userService) => _userService = userService;

        [HttpPost(nameof(Post))]
        public async Task<ObjectResult> Post([FromBody] SystemUser usuario)
        {
            var result = await _userService.SaveAsync(usuario);
            if (!result.Success)
                return BadRequest(result);

            return Created(nameof(Post), result);
        }

    }
}
