using E_Insurance_App.Models.DTOs;
using E_Insurance_App.Models.Entities;

namespace E_Insurance_App.Services.Interface
{
    public interface IInsurancePlanService
    {
        Task<InsurancePlan> RegisterPlanAsync(InsurancePlanDTO planDto);
        Task<InsurancePlan?> GetPlanByIdAsync(int planId);
    }
}
