using E_Insurance_App.Models.DTOs;

namespace E_Insurance_App.Repositories.Interface
{
    public interface IPolicyRepository
    {
        Task<int> CreatePolicyAsync(PolicyCreateDTO policyDTO);
        Task<PolicyResponseDTO> GetPolicyByIdAsync(int policyID);
        Task<List<PolicyViewDTO>> GetPoliciesByCustomerIDAsync(int customerID);
        Task<PolicyResponseDTO> PurchasePolicyAsync(PolicyPurchaseDTO policyDto);
    }
}
