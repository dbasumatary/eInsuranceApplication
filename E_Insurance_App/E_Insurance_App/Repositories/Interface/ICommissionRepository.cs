using E_Insurance_App.Models.DTOs;
using E_Insurance_App.Models.Entities;

namespace E_Insurance_App.Repositories.Interface
{
    public interface ICommissionRepository
    {
        Task<List<CommissionResponseDTO>> CalculateAgentCommissionAsync(int agentID);
        Task<List<CommissionResponseDTO>> GetAgentCommissionsAsync(int agentId);
        Task<List<Commission>> GetCommissionsByAgentIdAsync(int agentId);
        //Task PayMonthlyCommissionsAsync();
        Task PayCommissionsAsync(List<Commission> commissions);
    }
}
