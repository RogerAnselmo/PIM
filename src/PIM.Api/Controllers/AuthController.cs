using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using PIM.Api.Core.Services;
using PIM.Api.TransferObjects.Requests;

namespace PIM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost(nameof(Login))]
        public async Task<ObjectResult> Login([FromBody] LoginRequestModel loginRequestModel)
        {
            var user = await _authService.Authenticate(loginRequestModel);
            if (!user.Success)
                return BadRequest(user);

            return Ok(user);
        }
    }
}
