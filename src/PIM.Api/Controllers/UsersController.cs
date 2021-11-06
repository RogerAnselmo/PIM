using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using PIM.Api.Core.Models;
using PIM.Api.Core.Services;

namespace PIM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly SystemUserService _userService;

        public UsersController(SystemUserService userService) => _userService = userService;

        [HttpPost(nameof(CreateUser))]
        public async Task<ObjectResult> CreateUser([FromBody] SystemUser user)
        {
            var result = await _userService.SaveAsync(user);
            if (!result.Success)
                return BadRequest(result);

            return Created(nameof(CreateUser), result);
        }

    }
}
