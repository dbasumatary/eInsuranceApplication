using E_Insurance_App.Models.DTOs;
using E_Insurance_App.Models.Entities;
using E_Insurance_App.Repositories.Interface;
using E_Insurance_App.Services.Interface;

namespace E_Insurance_App.Services.Implementation
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ILogger<PaymentService> _logger;


        public PaymentService(IPaymentRepository paymentRepository, ILogger<PaymentService> logger)
        {
            _paymentRepository = paymentRepository;
            _logger = logger;
        }

        public async Task<PaymentResponseDTO> ProcessPaymentAsync(PaymentDTO paymentDTO)
        {
            _logger.LogInformation($"Processing payment for CustomerID: {paymentDTO.CustomerID}, PolicyID: {paymentDTO.PolicyID}");

            try
            {
                var payment = new Payment
                {
                    CustomerID = paymentDTO.CustomerID,
                    PolicyID = paymentDTO.PolicyID,
                    PremiumID = paymentDTO.PremiumID,
                    PaymentDate = paymentDTO.PaymentDate,
                    PaymentType = paymentDTO.PaymentType,
                };
                var result = await _paymentRepository.ProcessPaymentAsync(payment);

                _logger.LogInformation($"Payment processed successfully for CustomerID: {paymentDTO.CustomerID}, PolicyID: {paymentDTO.PolicyID}");
                return result;
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error during processing payment for CustomerID: {paymentDTO.CustomerID}, PolicyID: {paymentDTO.PolicyID}, Error: {ex.Message}");
                throw new Exception($"Error processing payment: {ex.Message}");
            }
        }


        public async Task<List<PaymentViewDTO>> GetPaymentsByCustomer(int customerID)
        {
            _logger.LogInformation($"Retrieving payments for CustomerID: {customerID}");

            try
            {
                var payments = await _paymentRepository.GetPaymentsByCustomerID(customerID);

                _logger.LogInformation($"Retrieved {payments.Count} payments for CustomerID: {customerID}");
                return payments;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during getting payments for CustomerID: {customerID}, Error: {ex.Message}");
                throw new Exception($"Error getting Payments by CustomerID: {ex.Message}");
            }
            
        }


        public async Task<PaymentViewDTO> GenerateReceiptAsync(int paymentId)
        {
            if (paymentId <= 0)
            {
                _logger.LogWarning("Invalid Payment ID received for receipt generation.");
                throw new ArgumentException("Invalid Payment ID.");
            }
            _logger.LogInformation($"Generating receipt for PaymentID: {paymentId}");

            try
            {
                var receipt = await _paymentRepository.GenerateReceiptAsync(paymentId);

                if (receipt == null)
                {
                    _logger.LogWarning($"Receipt not found for PaymentID: {paymentId}");
                    throw new Exception("Error : Receipt not found.");
                }

                _logger.LogInformation($"Receipt successfully generated for PaymentID: {paymentId}");
                return receipt;
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error during generating receipt for PaymentID: {paymentId}, Error: {ex.Message}");
                throw new Exception($"Error generating receipt: {ex.Message}");
            }
        }
    }
}
