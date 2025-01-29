using E_Insurance_App.Models.DTOs;
using E_Insurance_App.Models.Entities;

namespace E_Insurance_App.Services.Interface
{
    public interface IAgentService
    {
        Task<Agent> RegisterAgentAsync(AgentRegisterDTO agentDto);
        Task<Agent?> GetAgentByIdAsync(int agentId);
        Task<IEnumerable<Agent>> GetAllAgentsAsync();
        Task<Agent?> UpdateAgentAsync(int agentId, AgentUpdateDTO agentDto);
        Task<bool> DeleteAgentAsync(int agentId);
    }
}
