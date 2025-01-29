using E_Insurance_App.Models.Entities;

namespace E_Insurance_App.Repositories.Interface
{
    public interface IAgentRepository
    {
        Task<Agent> RegisterAgentAsync(Agent agent);
        Task<Agent?> GetAgentByIdAsync(int agentId);
        Task<IEnumerable<Agent>> GetAllAgentsAsync();
        Task<Agent?> UpdateAgentAsync(Agent agent);
        Task<bool> DeleteAgentAsync(int agentId);
    }
}
