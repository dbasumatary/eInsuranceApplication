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
    public class InsurancePlanController : ControllerBase
    {
        private readonly IInsurancePlanService _planService;
        private readonly ILogger<InsurancePlanController> _logger;

        public InsurancePlanController(IInsurancePlanService planService, ILogger<InsurancePlanController> logger)
        {
            _planService = planService;
            _logger = logger;
        }


        [HttpPost("InsurancePlan")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterPlan([FromBody] InsurancePlanDTO planDto)
        {
            _logger.LogInformation("Registering new insurance plan.");

            try
            {
                var plan = await _planService.RegisterPlanAsync(planDto);

                _logger.LogInformation("Insurance Plan registered successfully");
                return Ok(new { Message = "Plan registered successfully", InsurancePlan = plan });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering insurance plan.");
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpGet("{planId}")]
        public async Task<IActionResult> GetPlanById(int planId)
        {
            _logger.LogInformation("Retrieve insurance plan with ID: {PlanId}", planId);

            try
            {
                var plan = await _planService.GetPlanByIdAsync(planId);
                if (plan == null)
                {
                    _logger.LogWarning("Insurance Plan with ID: {PlanId} not found", planId);
                    return NotFound("Plan not found.");
                }
                var response = new
                {
                    plan.PlanID,
                    plan.PlanName,
                    plan.PlanDetails,
                    plan.CreatedAt
                };

                _logger.LogInformation($"Insurance Plan retrieved successfully for Id: {planId}");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving insurance plan of Id: {planId}");
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
