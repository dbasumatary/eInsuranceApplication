using E_Insurance_App.Models.DTOs;
using E_Insurance_App.Models.Entities;
using E_Insurance_App.Repositories.Interface;
using E_Insurance_App.Services.Interface;

namespace E_Insurance_App.Services.Implementation
{
    public class InsurancePlanService : IInsurancePlanService
    {
        private readonly IInsurancePlanRepository _planRepository;

        public InsurancePlanService(IInsurancePlanRepository planRepository)
        {
            _planRepository = planRepository;
        }


        public async Task<InsurancePlan> RegisterPlanAsync(InsurancePlanDTO planDto)
        {
            try 
            {
                var plan = new InsurancePlan
                {
                    PlanName = planDto.PlanName,
                    PlanDetails = planDto.PlanDetails
                };

                return await _planRepository.AddPlanAsync(plan);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error registering plans: {ex.Message}");
            }
        }


        public async Task<InsurancePlan?> GetPlanByIdAsync(int planId)
        {
            try
            {
                return await _planRepository.GetPlanByIdAsync(planId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting plans by Id: {ex.Message}");
            }
        }
    }
}
