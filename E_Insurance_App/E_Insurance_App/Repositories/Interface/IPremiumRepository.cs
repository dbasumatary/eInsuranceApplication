using E_Insurance_App.Models.DTOs;

namespace E_Insurance_App.Repositories.Interface
{
    public interface IPremiumRepository
    {
        Task<PremiumResponseDTO> CalculatePremiumAsync(PremiumCreateDTO premiumDTO);
    }
}
