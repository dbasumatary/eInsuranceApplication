using E_Insurance_App.Models.DTOs;
using E_Insurance_App.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Insurance_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommissionController : ControllerBase
    {
        private readonly ICommissionService _commissionService;

        public CommissionController(ICommissionService commissionService)
        {
            _commissionService = commissionService;
        }


        [HttpPost("calculate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CalculateAgentCommission([FromBody] CommissionDTO commission)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var commissions = await _commissionService.CalculateAgentCommissionAsync(commission.AgentID);

                if (commissions == null || commissions.Count == 0)
                    return NotFound(new { message = "No commissions found for this agent." });

                return Ok(commissions);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpGet("agent")]
        [Authorize(Roles = "Admin, Agent")]
        public async Task<IActionResult> GetAgentCommissions(int agentId)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var commissions = await _commissionService.GetAgentCommissionsAsync(agentId);
                if (commissions == null || commissions.Count == 0)
                {
                    return NotFound("No commissions found for this agent.");
                }
                return Ok(commissions);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
