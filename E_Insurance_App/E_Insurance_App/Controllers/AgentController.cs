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

        public AgentController(IAgentService agentService)
        {
            _agentService = agentService;
        }


        [HttpPost("register")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterAgent([FromBody] AgentRegisterDTO agentDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var agent = await _agentService.RegisterAgentAsync(agentDto);
                return Ok(new { Message = "Agent registered successfully", Agent = agent });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }



        [HttpGet("{agentId}")]
        [Authorize(Roles = "Admin,Agent")]
        public async Task<IActionResult> GetAgentById(int agentId)
        {
            try
            {
                var agent = await _agentService.GetAgentByIdAsync(agentId);
                if (agent == null)
                    return NotFound(new { Message = "Agent not found" });

                return Ok(agent);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllAgents()
        {
            try
            {
                var agents = await _agentService.GetAllAgentsAsync();
                return Ok(agents);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpPut("update/{agentId}")]
        [Authorize(Roles = "Admin,Agent")]
        public async Task<IActionResult> UpdateAgent(int agentId, [FromBody] AgentUpdateDTO agentDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var updatedAgent = await _agentService.UpdateAgentAsync(agentId, agentDto);
                if (updatedAgent == null)
                    return NotFound(new { Message = "Agent not found" });

                return Ok(new { Message = "Agent updated successfully", Agent = updatedAgent });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            
        }



        [HttpDelete("delete/{agentId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAgent(int agentId)
        {
            try
            {
                var deleted = await _agentService.DeleteAgentAsync(agentId);
                if (!deleted)
                    return NotFound(new { Message = "Agent not found" });

                return Ok(new { Message = "Agent deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }

        }
    }
}

