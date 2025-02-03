using E_Insurance_App.Models.DTOs;
using E_Insurance_App.Models.Entities;
using E_Insurance_App.Repositories.Interface;
using E_Insurance_App.Services.Interface;

namespace E_Insurance_App.Services.Implementation
{
    public class PolicyService : IPolicyService
    {
        private readonly IPolicyRepository _policyRepository;

        public PolicyService(IPolicyRepository policyRepository)
        {
            _policyRepository = policyRepository;
        }


        public async Task<PolicyResponseDTO> CreatePolicyAsync(PolicyCreateDTO policyDTO)
        {
            try
            {
                int policyID = await _policyRepository.CreatePolicyAsync(policyDTO);
                return await _policyRepository.GetPolicyByIdAsync(policyID);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating policy: {ex.Message}");
            }
            
        }

        public async Task<List<PolicyViewDTO>> GetCustomerPoliciesAsync(int customerID)
        {
            try
            {
                return await _policyRepository.GetPoliciesByCustomerIDAsync(customerID);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting Policies by CustomerID: {ex.Message}");
            }
            
        }

        public async Task<PolicyResponseDTO> PurchasePolicyAsync(PolicyPurchaseDTO policyDto)
        {
            try
            {
                return await _policyRepository.PurchasePolicyAsync(policyDto);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error during policy purchase: {ex.Message}");
            }
        }
    }
}
