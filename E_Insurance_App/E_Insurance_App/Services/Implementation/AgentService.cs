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

                return await _agentRepository.RegisterAgentAsync(agent);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error registering employee: {ex.Message}");
            }
        }


        public async Task<Agent?> GetAgentByIdAsync(int agentId)
        {
            try
            {
                return await _agentRepository.GetAgentByIdAsync(agentId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting  employee by Id: {ex.Message}");
            }
        }


        public async Task<IEnumerable<Agent>> GetAllAgentsAsync()
        {
            try
            {
                return await _agentRepository.GetAllAgentsAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting  employees list: {ex.Message}");
            }
        }


        public async Task<Agent?> UpdateAgentAsync(int agentId, AgentUpdateDTO agentDto)
        {
            try
            {
                var agent = await _agentRepository.GetAgentByIdAsync(agentId);
                if (agent == null)
                {
                    return null;
                }

                agent.FullName = agentDto.FullName;
                agent.Email = agentDto.Email;
                agent.CommissionRate = agentDto.CommissionRate;

                return await _agentRepository.UpdateAgentAsync(agent);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating employee: {ex.Message}");
            }
        }


        public async Task<bool> DeleteAgentAsync(int agentId)
        {
            try
            {
                return await _agentRepository.DeleteAgentAsync(agentId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting employee: {ex.Message}");
            }
        }
    }
}
