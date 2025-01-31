using E_Insurance_App.Models.DTOs;

namespace E_Insurance_App.Repositories.Interface
{
    public interface IPolicyRepository
    {
        Task<int> CreatePolicyAsync(PolicyCreateDTO policyDTO);
        Task<PolicyResponseDTO> GetPolicyByIdAsync(int policyID);
    }
}
