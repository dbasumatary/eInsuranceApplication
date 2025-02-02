using E_Insurance_App.Models.DTOs;
using E_Insurance_App.Repositories.Interface;
using E_Insurance_App.Services.Interface;

namespace E_Insurance_App.Services.Implementation
{
    public class CommissionService : ICommissionService
    {
        private readonly ICommissionRepository _commissionRepository;

        public CommissionService(ICommissionRepository commissionRepository)
        {
            _commissionRepository = commissionRepository;
        }

        public async Task<List<CommissionResponseDTO>> CalculateAgentCommissionAsync(int agentID)
        {
            try
            {
                return await _commissionRepository.CalculateAgentCommissionAsync(agentID);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating commission: {ex.Message}");
            }
        }
    }
}
