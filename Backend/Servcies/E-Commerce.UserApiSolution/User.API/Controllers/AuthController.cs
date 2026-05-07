using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using User.Application.DTOs;
using User.Application.Interfaces;

namespace User.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto dto)
        {
            var result = await _service.Login(dto);

            if (!result)
            {
                return Unauthorized("Invalid credentials");
            }

            return Ok("Login Success");
        }
    }
}
