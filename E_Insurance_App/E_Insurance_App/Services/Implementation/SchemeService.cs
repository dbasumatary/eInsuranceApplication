using E_Insurance_App.Models.DTOs;
using E_Insurance_App.Models.Entities;
using E_Insurance_App.Repositories.Interface;
using E_Insurance_App.Services.Interface;

namespace E_Insurance_App.Services.Implementation
{
    public class SchemeService : ISchemeService
    {
        private readonly ISchemeRepository _schemeRepository;
        private readonly IInsurancePlanRepository _planRepository;

        public SchemeService(ISchemeRepository schemeRepository, IInsurancePlanRepository planRepository)
        {
            _schemeRepository = schemeRepository;
            _planRepository = planRepository;
        }

        public async Task<Scheme> RegisterSchemeAsync(SchemeDTO schemeDto)
        {
            try
            {
                var plan = await _planRepository.GetPlanByIdAsync(schemeDto.PlanID);
                if (plan == null)
                {
                    throw new Exception("Insurance plan not found.");
                }

                var scheme = new Scheme
                {
                    SchemeName = schemeDto.SchemeName,
                    SchemeDetails = schemeDto.SchemeDetails,
                    SchemeFactor = schemeDto.SchemeFactor,
                    PlanID = schemeDto.PlanID
                };

                return await _schemeRepository.AddSchemeAsync(scheme);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error registering plans: {ex.Message}");
            }            
        }
    }
}
