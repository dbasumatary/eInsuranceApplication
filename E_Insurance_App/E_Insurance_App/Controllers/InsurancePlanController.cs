using E_Insurance_App.Models.DTOs;
using E_Insurance_App.Models.Entities;
using E_Insurance_App.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Insurance_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsurancePlanController : ControllerBase
    {
        private readonly IInsurancePlanService _planService;

        public InsurancePlanController(IInsurancePlanService planService)
        {
            _planService = planService;
        }


        [HttpPost]
        public async Task<IActionResult> RegisterPlan([FromBody] InsurancePlanDTO planDto)
        {
            try
            {
                var plan = await _planService.RegisterPlanAsync(planDto);
                //return CreatedAtAction(nameof(GetPlanById), new { planId = plan.PlanID }, plan);
                return Ok(new { Message = "Plan registered successfully", InsurancePlan = plan });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpGet("{planId}")]
        public async Task<IActionResult> GetPlanById(int planId)
        {
            try
            {
                var plan = await _planService.GetPlanByIdAsync(planId);
                if (plan == null) return NotFound("Plan not found.");
                return Ok(plan);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
