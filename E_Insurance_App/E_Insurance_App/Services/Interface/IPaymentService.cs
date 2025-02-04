using E_Insurance_App.Models.DTOs;
using E_Insurance_App.Models.Entities;

namespace E_Insurance_App.Services.Interface
{
    public interface IPaymentService
    {
        Task<PaymentResponseDTO> ProcessPaymentAsync(PaymentDTO paymentDTO);
        Task<List<PaymentViewDTO>> GetPaymentsByCustomer(int customerID);
        Task<PaymentViewDTO> GenerateReceiptAsync(int paymentId);
    }
}
