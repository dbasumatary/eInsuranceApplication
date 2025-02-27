﻿using E_Insurance_App.Models.DTOs;

namespace E_Insurance_App.Repositories.Interface
{
    public interface IPolicyRepository
    {
        Task<int> CreatePolicyAsync(PolicyCreateDTO policyDTO);
        Task<PolicyResponseDTO> GetPolicyByIdAsync(int policyID);
        Task<List<PolicyViewDTO>> GetPoliciesByCustomerIDAsync(int customerID);
        Task<PolicyResponseDTO> PurchasePolicyAsync(PolicyPurchaseDTO policyDto);

        Task<IEnumerable<PolicyResponseDTO>> GetAgentPoliciesAsync(int agentId);
        Task<List<PolicyResponseDTO>> SearchPoliciesAsync(PolicySearchDTO searchCriteria);

        Task<bool> CancelPolicyAsync(int policyId, string reason, string cancelledBy);
    }
}
