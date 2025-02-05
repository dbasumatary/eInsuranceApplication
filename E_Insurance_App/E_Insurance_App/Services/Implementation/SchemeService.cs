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
        private readonly ILogger<SchemeService> _logger;


        public SchemeService(ISchemeRepository schemeRepository, IInsurancePlanRepository planRepository, ILogger<SchemeService> logger)
        {
            _schemeRepository = schemeRepository;
            _planRepository = planRepository;
            _logger = logger;
        }

        public async Task<Scheme> RegisterSchemeAsync(SchemeDTO schemeDto)
        {
            _logger.LogInformation($"Registering scheme with name: {schemeDto.SchemeName} for PlanID: {schemeDto.PlanID}");

            try
            {
                var plan = await _planRepository.GetPlanByIdAsync(schemeDto.PlanID);
                if (plan == null)
                {
                    _logger.LogWarning($"Insurance plan with PlanID: {schemeDto.PlanID} not found.");
                    throw new Exception("Insurance plan not found.");
                }

                var scheme = new Scheme
                {
                    SchemeName = schemeDto.SchemeName,
                    SchemeDetails = schemeDto.SchemeDetails,
                    SchemeFactor = schemeDto.SchemeFactor,
                    PlanID = schemeDto.PlanID
                };

                var newScheme = await _schemeRepository.AddSchemeAsync(scheme);
                _logger.LogInformation($"Scheme with name: {newScheme.SchemeName} registered successfully for PlanID: {newScheme.PlanID}");

                return newScheme;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during registering scheme for PlanID: {schemeDto.PlanID}, Error: {ex.Message}");
                throw new Exception($"Error registering plans: {ex.Message}");
            }            
        }
    }
}
