using E_Insurance_App.Models.DTOs;
using E_Insurance_App.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Insurance_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(IAdminService adminService, ILogger<AdminController> logger)
        {
            _adminService = adminService;
            _logger = logger;   
        }

        [HttpPost("register")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterAdmin(AdminRegisterDTO adminDto)
        {
            _logger.LogInformation("Admin registration for {Username}", adminDto.Username);

            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var admin = await _adminService.RegisterAdminAsync(adminDto);
                return Ok(new { Message = "Admin registered successfully", Admin = admin });
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering admin: {Username}", adminDto.Username);
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
