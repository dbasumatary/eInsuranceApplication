using E_Insurance_App.Data;
using E_Insurance_App.Models.Entities;
using E_Insurance_App.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace E_Insurance_App.Repositories.Implementation
{
    public class InsurancePlanRepository : IInsurancePlanRepository
    {
        private readonly InsuranceDbContext _context;

        public InsurancePlanRepository(InsuranceDbContext context)
        {
            _context = context;
        }

        public async Task<InsurancePlan> AddPlanAsync(InsurancePlan plan)
        {
            try
            {
                _context.InsurancePlans.Add(plan);
                await _context.SaveChangesAsync();
                return plan;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding plan: {ex.Message}");
            }
        }


        public async Task<InsurancePlan?> GetPlanByIdAsync(int planId)
        {
            try
            {
                return await _context.InsurancePlans.Include(p => p.Schemes).FirstOrDefaultAsync(p => p.PlanID == planId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting plan by Id: {ex.Message}");
            }
        }
    }
}
