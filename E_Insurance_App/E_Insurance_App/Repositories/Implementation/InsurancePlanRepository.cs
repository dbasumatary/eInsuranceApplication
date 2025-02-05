using E_Insurance_App.Data;
using E_Insurance_App.Models.Entities;
using E_Insurance_App.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace E_Insurance_App.Repositories.Implementation
{
    public class InsurancePlanRepository : IInsurancePlanRepository
    {
        private readonly InsuranceDbContext _context;
        private readonly ILogger<InsurancePlanRepository> _logger;


        public InsurancePlanRepository(InsuranceDbContext context, ILogger<InsurancePlanRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<InsurancePlan> AddPlanAsync(InsurancePlan plan)
        {
            _logger.LogInformation("Adding a new insurance plan: {PlanName}", plan.PlanName);

            try
            {
                _context.InsurancePlans.Add(plan);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Insurance plan added successfully: {PlanId}", plan.PlanID);
                return plan;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding plan: {PlanName}", plan.PlanName);
                throw new Exception($"Error adding plan: {ex.Message}");
            }
        }


        public async Task<InsurancePlan?> GetPlanByIdAsync(int planId)
        {
            _logger.LogInformation("Retrieving insurance plan with ID: {PlanId}", planId);

            try
            {
                var plan = await _context.InsurancePlans.Include(p => p.Schemes).FirstOrDefaultAsync(p => p.PlanID == planId);
                if (plan == null)
                {
                    _logger.LogWarning("No insurance plan found with ID: {PlanId}", planId);
                }
                return plan;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting plan by Id: {PlanId}", planId);
                throw new Exception($"Error getting plan by Id: {ex.Message}");
            }
        }
    }
}
