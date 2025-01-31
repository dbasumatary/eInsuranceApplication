using E_Insurance_App.Models.DTOs;
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
    }
}
