using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Middleware;
using StudentManagementSystem.Models.DTOs;
using StudentManagementSystem.Services;

namespace StudentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;

        public AuthController(ITokenService tokenService, IConfiguration config)
        {
            _tokenService = tokenService;
            _config = config;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<LoginResponseDto>), StatusCodes.Status200OK)]
        public IActionResult Login([FromBody] LoginDto request)
        {
            var validUsername = _config["AdminUser:Username"];
            var validPassword = _config["AdminUser:Password"];

            if (request.Username != validUsername || request.Password != validPassword)
            {
                throw new UnauthorizedAppException("Invalid username or password.");
            }

            var result = _tokenService.GenerateToken(request.Username);
            return Ok(ApiResponse<LoginResponseDto>.Ok(result, "Login successful"));
        }
    }
}
