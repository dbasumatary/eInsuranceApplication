using E_Insurance_App.Models.DTOs;
using E_Insurance_App.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Insurance_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAdmin(AdminRegisterDTO adminDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var admin = await _adminService.RegisterAdminAsync(adminDto);
            return Ok(new { Message = "Admin registered successfully", Admin = admin });
        }
    }
}
