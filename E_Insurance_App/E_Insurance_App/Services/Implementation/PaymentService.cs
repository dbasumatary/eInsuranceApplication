using E_Insurance_App.Models.DTOs;
using E_Insurance_App.Models.Entities;
using E_Insurance_App.Repositories.Interface;
using E_Insurance_App.Services.Interface;

namespace E_Insurance_App.Services.Implementation
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<PaymentResponseDTO> ProcessPaymentAsync(PaymentDTO paymentDTO)
        {
            var payment = new Payment
            {
                CustomerID = paymentDTO.CustomerID,
                PolicyID = paymentDTO.PolicyID,
                PremiumID = paymentDTO.PremiumID,
                PaymentDate = paymentDTO.PaymentDate,
                PaymentType = paymentDTO.PaymentType,
            };
            return await _paymentRepository.ProcessPaymentAsync(payment);
        }


        public async Task<List<PaymentViewDTO>> GetPaymentsByCustomer(int customerID)
        {
            try
            {
                return await _paymentRepository.GetPaymentsByCustomerID(customerID);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting Payments by CustomerID: {ex.Message}");
            }
            
        }
    }
}
