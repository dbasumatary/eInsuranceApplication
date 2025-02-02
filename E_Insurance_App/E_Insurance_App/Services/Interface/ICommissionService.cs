using E_Insurance_App.Models.DTOs;

namespace E_Insurance_App.Services.Interface
{
    public interface ICommissionService
    {
        Task<List<CommissionResponseDTO>> CalculateAgentCommissionAsync(int agentID);
        Task<List<CommissionResponseDTO>> GetAgentCommissionsAsync(int agentId);
    }
}
