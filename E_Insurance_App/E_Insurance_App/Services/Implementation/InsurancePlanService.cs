using E_Insurance_App.Models.DTOs;
using E_Insurance_App.Models.Entities;
using E_Insurance_App.Repositories.Interface;
using E_Insurance_App.Services.Interface;
using System.Runtime.CompilerServices;

namespace E_Insurance_App.Services.Implementation
{
    public class InsurancePlanService : IInsurancePlanService
    {
        private readonly IInsurancePlanRepository _planRepository;
        private readonly ILogger<InsurancePlanService> _logger;

        public InsurancePlanService(IInsurancePlanRepository planRepository, ILogger<InsurancePlanService> logger)
        {
            _planRepository = planRepository;
            _logger = logger;
        }


        public async Task<InsurancePlan> RegisterPlanAsync(InsurancePlanDTO planDto)
        {
            _logger.LogInformation($"Registering new insurance plan with PlanName: {planDto.PlanName}");

            try
            {
                var plan = new InsurancePlan
                {
                    PlanName = planDto.PlanName,
                    PlanDetails = planDto.PlanDetails
                };

                var result = await _planRepository.AddPlanAsync(plan);

                _logger.LogInformation($"Insurance plan with PlanName: {planDto.PlanName} successfully registered.");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during registering insurance plan with PlanName: {planDto.PlanName}, Error: {ex.Message}");
                throw new Exception($"Error registering plans: {ex.Message}");
            }
        }


        public async Task<InsurancePlan?> GetPlanByIdAsync(int planId)
        {
            _logger.LogInformation($"Retrieving insurance plan details for PlanID: {planId}");

            try
            {
                var plan = await _planRepository.GetPlanByIdAsync(planId);

                if (plan == null)
                {
                    _logger.LogWarning($"Insurance plan with PlanID: {planId} not found.");
                }

                return plan;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during getting insurance plan by PlanID: {planId}, Error: {ex.Message}");
                throw new Exception($"Error getting plans by Id: {ex.Message}");
            }
        }
    }
}
