using E_Insurance_App.Models.DTOs;

namespace E_Insurance_App.Repositories.Interface
{
    public interface ICommissionRepository
    {
        Task<List<CommissionResponseDTO>> CalculateAgentCommissionAsync(int agentID);
        Task<List<CommissionResponseDTO>> GetAgentCommissionsAsync(int agentId);
    }
}
