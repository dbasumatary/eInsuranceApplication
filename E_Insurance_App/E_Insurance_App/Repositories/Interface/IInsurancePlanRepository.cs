using E_Insurance_App.Models.Entities;

namespace E_Insurance_App.Repositories.Interface
{
    public interface IInsurancePlanRepository
    {
        Task<InsurancePlan> AddPlanAsync(InsurancePlan plan);
        Task<InsurancePlan?> GetPlanByIdAsync(int planId);
    }
}
