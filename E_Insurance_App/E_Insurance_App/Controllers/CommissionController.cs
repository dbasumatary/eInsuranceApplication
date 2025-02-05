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
        private readonly ILogger<CommissionController> _logger;

        public CommissionController(ICommissionService commissionService, ILogger<CommissionController> logger)
        {
            _commissionService = commissionService;
            _logger = logger;
        }


        [HttpPost("calculate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CalculateAgentCommission([FromBody] CommissionDTO commission)
        {
            _logger.LogInformation("CalculateAgentCommission for AgentID: {AgentID}", commission.AgentID);

            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for Agent commission calculation.");
                    return BadRequest(ModelState);
                }

                var commissions = await _commissionService.CalculateAgentCommissionAsync(commission.AgentID);

                if (commissions == null || commissions.Count == 0)
                {
                    _logger.LogWarning("No commissions found for AgentID: {AgentID}", commission.AgentID);
                    return NotFound(new { message = "No commissions found." });
                }

                _logger.LogInformation("Successfully calculated commission for AgentID: {AgentID}", commission.AgentID);
                return Ok(commissions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating commission for AgentID: {AgentID}", commission.AgentID);
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpGet("agent")]
        [Authorize(Roles = "Admin, Agent")]
        public async Task<IActionResult> GetAgentCommissions(int agentId)
        {
            _logger.LogInformation("GetAgentCommissions for AgentID: {AgentID}", agentId);

            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var commissions = await _commissionService.GetAgentCommissionsAsync(agentId);
                if (commissions == null || commissions.Count == 0)
                {
                    _logger.LogWarning("No commissions found for AgentID: {AgentID}", agentId);
                    return NotFound("No commissions found.");
                }

                _logger.LogInformation("Commissions retrieved successfully for AgentID: {AgentID}", agentId);
                return Ok(commissions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: Couldn't retrieve commission for AgentID: {AgentID}", agentId);
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpPost("payment")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PayAgentCommission([FromBody] PayCommissionDTO payCommission)
        {
            _logger.LogInformation("PayAgentCommission AgentID: {AgentID}", payCommission.AgentID);

            if (payCommission == null || payCommission.AgentID <= 0 || payCommission.CommissionIDs == null || payCommission.CommissionIDs.Count == 0)
            {
                _logger.LogWarning("Invalid commission details provided.");
                return BadRequest(new { error = "Invalid commission details provided." });
            }

            try
            {
                var result = await _commissionService.PayAgentCommissionAsync(payCommission.AgentID, payCommission.CommissionIDs);

                if (result)
                {
                    _logger.LogInformation("Commissions paid successfully for AgentID: {AgentID}", payCommission.AgentID);
                    return Ok(new { message = "Commissions paid successfully." });
                }

                else
                {
                    _logger.LogWarning("No commissions found or all were already paid for AgentID: {AgentID}", payCommission.AgentID);
                    return NotFound(new { message = "No commissions found or all commissions were already paid." });
                }
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: Couldn't process payment for AgentID: {AgentID}", payCommission.AgentID);
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}

