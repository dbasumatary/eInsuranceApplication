using E_Insurance_App.Models.DTOs;

namespace E_Insurance_App.Services.Interface
{
    public interface IPremiumService
    {
        Task<PremiumResponseDTO> CalculatePremiumAsync(PremiumCreateDTO premiumDTO);

    }
}
