using E_Insurance_App.Models.DTOs;
using E_Insurance_App.Repositories.Interface;
using E_Insurance_App.Services.Interface;

namespace E_Insurance_App.Services.Implementation
{
    public class PremiumService : IPremiumService
    {
        private readonly IPremiumRepository _premiumRepository;
        private readonly ILogger<PremiumService> _logger;

        public PremiumService(IPremiumRepository premiumRepository, ILogger<PremiumService> logger)
        {
            _premiumRepository = premiumRepository;
            _logger = logger;
        }

        public async Task<PremiumResponseDTO> CalculatePremiumAsync(PremiumCreateDTO premiumDTO)
        {
            _logger.LogInformation($"Calculating premium for PolicyID: {premiumDTO.PolicyID} with details: {premiumDTO}");

            try
            {
                var premiumResponse = await _premiumRepository.CalculatePremiumAsync(premiumDTO);

                _logger.LogInformation($"Premium calculated successfully for PolicyID: {premiumDTO.PolicyID}");

                return premiumResponse;
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error during calculating premium for PolicyID: {premiumDTO.PolicyID}, Error: {ex.Message}");
                throw new Exception($"Error calculating Premium: {ex.Message}");
            }
            
        }
    }
}

