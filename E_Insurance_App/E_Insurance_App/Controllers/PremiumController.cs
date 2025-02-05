using E_Insurance_App.Models.DTOs;
using E_Insurance_App.Models.Entities;
using E_Insurance_App.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Insurance_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PremiumController : ControllerBase
    {
        private readonly IPremiumService _premiumService;
        private readonly ILogger<PremiumController> _logger;

        public PremiumController(IPremiumService premiumService, ILogger<PremiumController> logger)
        {
            _premiumService = premiumService;
            _logger = logger;
        }


        [HttpPost("calculate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CalculatePremium([FromBody] PremiumCreateDTO premiumDTO)
        {
            _logger.LogInformation("Calculating Premium for CustomerId: {CustomerID}", premiumDTO.CustomerID);

            try
            {
                var premium = await _premiumService.CalculatePremiumAsync(premiumDTO);

                _logger.LogInformation("Premium registered successfully for CustomerId : {CustomerID}", premiumDTO.CustomerID);
                return Ok(premium);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating premium for CustomerId : {CustomerID}", premiumDTO.CustomerID);
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}

