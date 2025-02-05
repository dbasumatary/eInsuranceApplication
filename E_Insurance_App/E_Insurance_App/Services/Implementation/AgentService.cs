using E_Insurance_App.Models.DTOs;
using E_Insurance_App.Models.Entities;
using E_Insurance_App.Repositories.Interface;
using E_Insurance_App.Services.Interface;

namespace E_Insurance_App.Services.Implementation
{
    public class AgentService : IAgentService
    {
        private readonly IAgentRepository _agentRepository;
        private readonly ILogger<AgentService> _logger;

        public AgentService(IAgentRepository agentRepository, ILogger<AgentService> logger)
        {
            _agentRepository = agentRepository;
            _logger = logger;
        }

        public async Task<Agent> RegisterAgentAsync(AgentRegisterDTO agentDto)
        {
            _logger.LogInformation("Registering new agent with username: {Username}", agentDto.Username);

            try
            {
                var agent = new Agent
                {
                    Username = agentDto.Username,
                    Password = BCrypt.Net.BCrypt.HashPassword(agentDto.Password),
                    Email = agentDto.Email,
                    FullName = agentDto.FullName,
                    CommissionRate = agentDto.CommissionRate
                };

                var regAgent = await _agentRepository.RegisterAgentAsync(agent);

                _logger.LogInformation("Agent registered successfully with ID: {AgentID}", regAgent.AgentID);
                return regAgent;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during register agent with username: {Username}", agentDto.Username);
                throw new Exception($"Error registering agents: {ex.Message}");
            }
        }


        public async Task<Agent?> GetAgentByIdAsync(int agentId)
        {
            _logger.LogInformation("Retrieving agent by ID: {AgentID}", agentId);

            try
            {
                var agent = await _agentRepository.GetAgentByIdAsync(agentId);
                if (agent == null)
                {
                    _logger.LogWarning("Agent not found with ID: {AgentID}", agentId);
                }

                return agent;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during getting agent by ID: {AgentID}", agentId);
                throw new Exception($"Error getting agents by Id: {ex.Message}");
            }
        }


        public async Task<IEnumerable<Agent>> GetAllAgentsAsync()
        {
            _logger.LogInformation("Retrieving list of all agents.");

            try
            {
                var agents = await _agentRepository.GetAllAgentsAsync();
                _logger.LogInformation("Retrieved {AgentCount} agents.", agents.Count());

                return agents;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during getting all agents.");
                throw new Exception($"Error getting  agents list: {ex.Message}");
            }
        }


        public async Task<Agent?> UpdateAgentAsync(int agentId, AgentUpdateDTO agentDto)
        {
            _logger.LogInformation("Updating agent with ID: {AgentID}", agentId);

            try
            {
                var agent = await _agentRepository.GetAgentByIdAsync(agentId);
                if (agent == null)
                {
                    _logger.LogWarning("Agent not found with ID: {AgentID}", agentId);
                    return null;
                }

                agent.FullName = agentDto.FullName;
                agent.Email = agentDto.Email;
                agent.CommissionRate = agentDto.CommissionRate;

                var updatedAgent = await _agentRepository.UpdateAgentAsync(agent);

                _logger.LogInformation("Agent updated successfully with ID: {AgentID}", updatedAgent.AgentID);
                return updatedAgent;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during updating agent with ID: {AgentID}", agentId);
                throw new Exception($"Error updating agent: {ex.Message}");
            }
        }


        public async Task<bool> DeleteAgentAsync(int agentId)
        {
            _logger.LogInformation("Deleting agent with ID: {AgentID}", agentId);

            try
            {
                var result = await _agentRepository.DeleteAgentAsync(agentId);

                if (result)
                {
                    _logger.LogInformation("Agent deleted successfully with ID: {AgentID}", agentId);
                }
                else
                {
                    _logger.LogWarning("Failed to delete agent with ID: {AgentID}", agentId);
                }
                return result;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during deleting agent with ID: {AgentID}", agentId);
                throw new Exception($"Error deleting agent: {ex.Message}");
            }
        }
    }
}
