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


        public async Task<List<CommissionResponseDTO>> GetAgentCommissionsAsync(int agentId)
        {
            try
            {
                return await _commissionRepository.GetAgentCommissionsAsync(agentId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting commission: {ex.Message}");
            }
            
        }


        public async Task<bool> PayAgentCommissionAsync(int agentId, List<int> commissionIds)
        {
            var agentCommissions = await _commissionRepository.GetCommissionsByAgentIdAsync(agentId);

            var commissionsToUpdate = agentCommissions
                .Where(c => commissionIds.Contains(c.CommissionID) && !c.IsPaid)
                .ToList();

            if (commissionsToUpdate.Count == 0)
            {
                return false;
            }

            //Updating columns
            foreach (var commission in commissionsToUpdate)
            {
                commission.IsPaid = true;
                commission.PaymentProcessedDate = DateTime.UtcNow; 
            }

            await _commissionRepository.PayCommissionsAsync(commissionsToUpdate);

            return true;
        }
    }
}

