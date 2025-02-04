using E_Insurance_App.Models.DTOs;

namespace E_Insurance_App.Services.Interface
{
    public interface IPolicyService
    {
        Task<PolicyResponseDTO> CreatePolicyAsync(PolicyCreateDTO policyDTO);
        Task<List<PolicyViewDTO>> GetCustomerPoliciesAsync(int customerID);
        Task<PolicyResponseDTO> PurchasePolicyAsync(PolicyPurchaseDTO policyDto);
        Task<IEnumerable<PolicyResponseDTO>> GetAgentPoliciesAsync(int agentId);
        Task<List<PolicyResponseDTO>> SearchPoliciesAsync(PolicySearchDTO searchCriteria);

        Task<bool> CancelPolicyAsync(int policyId, string reason, string cancelledBy);
        
        }
}
