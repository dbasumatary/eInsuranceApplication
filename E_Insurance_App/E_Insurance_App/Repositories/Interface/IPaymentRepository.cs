using E_Insurance_App.Models.DTOs;
using E_Insurance_App.Models.Entities;

namespace E_Insurance_App.Repositories.Interface
{
    public interface IPaymentRepository
    {
        Task<PaymentResponseDTO> ProcessPaymentAsync(Payment request);
        Task<List<PaymentViewDTO>> GetPaymentsByCustomerID(int customerID);
    }
}
