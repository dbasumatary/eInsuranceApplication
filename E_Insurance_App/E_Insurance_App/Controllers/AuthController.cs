using E_Insurance_App.Models.DTOs;
using E_Insurance_App.Repositories.Interface;
using E_Insurance_App.Utilities.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Insurance_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtService jwtService, ILogger<AuthController> logger)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for username: {Username}", loginDto.Username);
                return BadRequest(ModelState);
            }

            var user = await _userRepository.GetUserByUsernameAsync(loginDto.Username);

            if (user == null || !_passwordHasher.VerifyPassword(loginDto.Password, user.PasswordHash))
            {
                _logger.LogWarning("Failed login attempt for username: {Username}", loginDto.Username);
                return Unauthorized(new { Message = "Invalid credentials" });
            }

            var token = _jwtService.GenerateToken(user.Username, user.Role);

            _logger.LogInformation("User {Username} logged in successfully.", loginDto.Username);
            return Ok(new { Token = token });
        }
    }
}
