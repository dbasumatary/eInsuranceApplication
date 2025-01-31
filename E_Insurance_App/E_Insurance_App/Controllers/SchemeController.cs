using E_Insurance_App.Models.DTOs;
using E_Insurance_App.Models.Entities;
using E_Insurance_App.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace E_Insurance_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchemeController : ControllerBase
    {
        private readonly ISchemeService _schemeService;

        public SchemeController(ISchemeService schemeService)
        {
            _schemeService = schemeService;
        }

        [HttpPost("register-scheme")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterScheme([FromBody] SchemeDTO schemeDto)
        {
            try
            {
                var scheme = await _schemeService.RegisterSchemeAsync(schemeDto);

                var response = new SchemeResponseDTO
                {
                    SchemeID = scheme.SchemeID,
                    SchemeName = scheme.SchemeName,
                    SchemeDetails = scheme.SchemeDetails,
                    SchemeFactor = scheme.SchemeFactor,
                    PlanID = scheme.PlanID
                };

                return Ok(new { Message = "Scheme registered successfully", response });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
