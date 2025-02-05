using E_Insurance_App.Models.DTOs;
using E_Insurance_App.Models.Entities;
using E_Insurance_App.Repositories.Interface;
using E_Insurance_App.Services.Interface;

namespace E_Insurance_App.Services.Implementation
{
    public class PolicyService : IPolicyService
    {
        private readonly IPolicyRepository _policyRepository;
        private readonly ILogger<PolicyService> _logger;

        public PolicyService(IPolicyRepository policyRepository, ILogger<PolicyService> logger)
        {
            _policyRepository = policyRepository;
            _logger = logger;
        }


        public async Task<PolicyResponseDTO> CreatePolicyAsync(PolicyCreateDTO policyDTO)
        {
            _logger.LogInformation($"Creating policy for CustomerID: {policyDTO.CustomerID}");

            try
            {
                int policyID = await _policyRepository.CreatePolicyAsync(policyDTO);
                
                var policyResponse = await _policyRepository.GetPolicyByIdAsync(policyID);

                _logger.LogInformation($"Policy created successfully with PolicyID: {policyID}");
                return policyResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during creating policy for CustomerID: {policyDTO.CustomerID}, Error: {ex.Message}");
                throw new Exception($"Error creating policy: {ex.Message}");
            }
            
        }

        public async Task<List<PolicyViewDTO>> GetCustomerPoliciesAsync(int customerID)
        {
            _logger.LogInformation($"Retrieving policies for CustomerID: {customerID}");

            try
            {
                var policies = await _policyRepository.GetPoliciesByCustomerIDAsync(customerID);

                if (policies == null || policies.Count == 0)
                {
                    _logger.LogWarning($"No policies found for CustomerID: {customerID}");
                }

                _logger.LogInformation($"Retrieved {policies.Count} policies for CustomerID: {customerID}");
                return policies;
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error during getting policies for CustomerID: {customerID}, Error: {ex.Message}");
                throw new Exception($"Error getting Policies by CustomerID: {ex.Message}");
            }
            
        }


        public async Task<PolicyResponseDTO> PurchasePolicyAsync(PolicyPurchaseDTO policyDto)
        {
            _logger.LogInformation($"Purchasing policy for CustomerID: {policyDto.CustomerID}");

            try
            {
                var policyResponse = await _policyRepository.PurchasePolicyAsync(policyDto);

                _logger.LogInformation($"Policy purchased successfully for CustomerID: {policyDto.CustomerID}");
                return policyResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during policy purchase for CustomerID: {policyDto.CustomerID}, Error: {ex.Message}");
                throw new Exception($"Error during policy purchase: {ex.Message}");
            }
        }

        public async Task<IEnumerable<PolicyResponseDTO>> GetAgentPoliciesAsync(int agentId)
        {
            _logger.LogInformation($"Retrieving policies for AgentID: {agentId}");

            try
            {
                var policies = await _policyRepository.GetAgentPoliciesAsync(agentId);

                if (policies == null || !policies.Any())
                {
                    _logger.LogWarning($"No policies found for AgentID: {agentId}");
                }

                _logger.LogInformation($"Retrieved {policies.Count()} policies for AgentID: {agentId}");
                return policies;
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error during retrieving policies for AgentID: {agentId}, Error: {ex.Message}");
                throw new Exception($"Error retrieving policies of agent: {ex.Message}");
            }
        }


        public async Task<List<PolicyResponseDTO>> SearchPoliciesAsync(PolicySearchDTO searchCriteria)
        {
            _logger.LogInformation($"Searching policies with criteria: {searchCriteria}");

            try
            {
                var policies = await _policyRepository.SearchPoliciesAsync(searchCriteria);

                _logger.LogInformation($"Found {policies.Count} policies matching the search criteria.");
                return policies;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during retrieving policies with search criteria: {searchCriteria}, Error: {ex.Message}");
                throw new Exception($"Error retrieving policies: {ex.Message}");
            }
        }


        public async Task<bool> CancelPolicyAsync(int policyId, string reason, string cancelledBy)
        {
            _logger.LogInformation($"Cancelling policy with PolicyID: {policyId}, Reason: {reason}");

            try
            {
                var isCancelled = await _policyRepository.CancelPolicyAsync(policyId, reason, cancelledBy);

                if (isCancelled)
                {
                    _logger.LogInformation($"Policy with PolicyID: {policyId} cancelled successfully.");
                }
                else
                {
                    _logger.LogWarning($"Failed to cancel policy with PolicyID: {policyId}.");
                }

                return isCancelled;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during processing cancellation for PolicyID: {policyId}, Reason: {reason}, Error: {ex.Message}");
                throw new Exception($"Error processing policy cancellation: {ex.Message}");
            }
        }

    }
}
