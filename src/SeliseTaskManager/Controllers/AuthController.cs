using Microsoft.AspNetCore.Mvc;
using SeliseTaskManager.Application.Auth;
using SeliseTaskManager.Application.Interfaces.Services;
using SeliseTaskManager.Infrastructure.Common.Interfaces;

namespace SeliseTaskManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthController(
            IUserService userService, 
            IJwtTokenService jwtTokenService)
        {
            _userService = userService;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(TokenRequest tokenRequest)
        {
            var user = await _userService.GetUserAsync(tokenRequest.Email, tokenRequest.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            var token = _jwtTokenService.GenerateToken(user.Id, user.Email, user.Role);

            return Ok(token);
        }
    }
}
