using E_Insurance_App.Models.DTOs;
using E_Insurance_App.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Insurance_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentController : ControllerBase
    {
        private readonly IAgentService _agentService;
        private readonly ILogger<AgentController> _logger;

        public AgentController(IAgentService agentService, ILogger<AgentController> logger)
        {
            _agentService = agentService;
            _logger = logger;
        }


        [HttpPost("register")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterAgent([FromBody] AgentRegisterDTO agentDto)
        {
            _logger.LogInformation("RegisterAgent for {Username}", agentDto.Username);

            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for {Username}", agentDto.Username);
                    return BadRequest(ModelState);
                }

                var agent = await _agentService.RegisterAgentAsync(agentDto);

                _logger.LogInformation("Agent {Username} registered successfully", agentDto.Username);
                return Ok(new { Message = "Agent registered successfully", Agent = agent });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: Couldn't register agent {Username}", agentDto.Username);
                return BadRequest(new { error = ex.Message });
            }
        }



        [HttpGet("{agentId}")]
        [Authorize(Roles = "Admin,Agent")]
        public async Task<IActionResult> GetAgentById(int agentId)
        {
            _logger.LogInformation("GetAgentById for AgentID: {AgentID}", agentId);

            try
            {
                var agent = await _agentService.GetAgentByIdAsync(agentId);
                if (agent == null)
                {
                    _logger.LogWarning("Agent not found for AgentID: {AgentID}", agentId);
                    return NotFound(new { Message = "Agent not found" });
                }

                _logger.LogInformation("Agent details retrieved successfully for AgentID: {AgentID}", agentId);
                return Ok(agent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: Couldn't retrieve agent for AgentID: {AgentID}", agentId);
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllAgents()
        {
            _logger.LogInformation("GetAllAgents for all agents.");

            try
            {
                var agents = await _agentService.GetAllAgentsAsync();

                _logger.LogInformation("All agents retrieved successfully.");
                return Ok(agents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all agents.");
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpPut("update/{agentId}")]
        [Authorize(Roles = "Admin,Agent")]
        public async Task<IActionResult> UpdateAgent(int agentId, [FromBody] AgentUpdateDTO agentDto)
        {
            _logger.LogInformation("UpdateAgent for AgentID: {AgentID}", agentId);

            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state in UpdateAgent for AgentID: {AgentID}", agentId);
                    return BadRequest(ModelState);
                }

                var updatedAgent = await _agentService.UpdateAgentAsync(agentId, agentDto);
                if (updatedAgent == null)
                {
                    _logger.LogWarning("Agent not found for updating. AgentID: {AgentID}", agentId);
                    return NotFound(new { Message = "Agent not found" });
                }

                _logger.LogInformation("Agent updated successfully for AgentID: {AgentID}", agentId);
                return Ok(new { Message = "Agent updated successfully", Agent = updatedAgent });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: Couldn't update agent for AgentID: {AgentID}", agentId);
                return BadRequest(new { error = ex.Message });
            }
            
        }



        [HttpDelete("delete/{agentId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAgent(int agentId)
        {
            _logger.LogInformation("DeleteAgent for AgentID: {AgentID}", agentId);

            try
            {
                var deleted = await _agentService.DeleteAgentAsync(agentId);
                if (!deleted)
                {
                    _logger.LogWarning("Agent not found to deleting. AgentID: {AgentID}", agentId);
                    return NotFound(new { Message = "Agent not found" });
                }

                _logger.LogInformation("Agent deleted successfully. AgentID: {AgentID}", agentId);
                return Ok(new { Message = "Agent deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: Couldn't delete agent for AgentID: {AgentID}", agentId);
                return BadRequest(new { error = ex.Message });
            }

        }
    }
}

