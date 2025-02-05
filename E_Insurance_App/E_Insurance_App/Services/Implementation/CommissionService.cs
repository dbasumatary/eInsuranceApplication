using E_Insurance_App.Models.DTOs;
using E_Insurance_App.Repositories.Interface;
using E_Insurance_App.Services.Interface;

namespace E_Insurance_App.Services.Implementation
{
    public class CommissionService : ICommissionService
    {
        private readonly ICommissionRepository _commissionRepository;
        private readonly ILogger<CommissionService> _logger;


        public CommissionService(ICommissionRepository commissionRepository, ILogger<CommissionService> logger)
        {
            _commissionRepository = commissionRepository;
            _logger = logger;
        }


        public async Task<List<CommissionResponseDTO>> CalculateAgentCommissionAsync(int agentID)
        {
            _logger.LogInformation($"Calculating commission for AgentID: {agentID}");

            try
            {
                return await _commissionRepository.CalculateAgentCommissionAsync(agentID);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during calculating commission for AgentID {agentID}: {ex.Message}");
                throw new Exception($"Error creating commission: {ex.Message}");
            }
        }


        public async Task<List<CommissionResponseDTO>> GetAgentCommissionsAsync(int agentId)
        {
            _logger.LogInformation($"Retrieving commission details for AgentID: {agentId}");

            try
            {
                return await _commissionRepository.GetAgentCommissionsAsync(agentId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during getting commission for AgentID {agentId}: {ex.Message}");
                throw new Exception($"Error getting commission: {ex.Message}");
            }
            
        }


        public async Task<bool> PayAgentCommissionAsync(int agentId, List<int> commissionIds)
        {
            _logger.LogInformation($"Initiating commission payment for AgentID: {agentId}");

            try
            {
                var agentCommissions = await _commissionRepository.GetCommissionsByAgentIdAsync(agentId);

                var commissionsToUpdate = agentCommissions
                    .Where(c => commissionIds.Contains(c.CommissionID) && !c.IsPaid)
                    .ToList();

                if (commissionsToUpdate.Count == 0)
                {
                    _logger.LogWarning($"No unpaid commissions found for AgentID: {agentId}");
                    return false;
                }

                //Updating columns
                foreach (var commission in commissionsToUpdate)
                {
                    commission.IsPaid = true;
                    commission.PaymentProcessedDate = DateTime.UtcNow;
                }

                await _commissionRepository.PayCommissionsAsync(commissionsToUpdate);

                _logger.LogInformation($"Successfully processed commission payment for AgentID: {agentId}");
                return true;
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error during paying commission for AgentID {agentId}: {ex.Message}");
                throw new Exception($"Error processing commission payment: {ex.Message}");
            }
        }
    }
}

