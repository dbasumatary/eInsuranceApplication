using E_Insurance_App.Models.DTOs;
using E_Insurance_App.Repositories.Interface;
using E_Insurance_App.Services.Interface;

namespace E_Insurance_App.Services.Implementation
{
    public class PremiumService : IPremiumService
    {
        private readonly IPremiumRepository _premiumRepository;

        public PremiumService(IPremiumRepository premiumRepository)
        {
            _premiumRepository = premiumRepository;
        }

        public async Task<PremiumResponseDTO> CalculatePremiumAsync(PremiumCreateDTO premiumDTO)
        {
            try
            {
                return await _premiumRepository.CalculatePremiumAsync(premiumDTO);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error calculating Premium: {ex.Message}");
            }
            
        }
    }
}

