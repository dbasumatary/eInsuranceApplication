using E_Insurance_App.Models.DTOs;
using E_Insurance_App.Models.Entities;
using E_Insurance_App.Repositories.Interface;
using E_Insurance_App.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using System.Reflection.Metadata;

namespace E_Insurance_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }


        [HttpPost("process")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentDTO paymentDto)
        {
            _logger.LogInformation("Processing payment.");

            try
            {                 
                var response = await _paymentService.ProcessPaymentAsync(paymentDto);
               
                _logger.LogInformation("Payment registered successfully");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment.");
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpGet("View")]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> GetPaymentsByCustomer(int customerID)
        {
            _logger.LogInformation("Retrieve payments for customer with ID: {CustomerId}", customerID);

            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var payments = await _paymentService.GetPaymentsByCustomer(customerID);

                if (payments == null || payments.Count == 0)
                {
                    _logger.LogWarning("Payment for ID: {CustomerId} not found", customerID);
                    return NotFound(new { message = "No payments found for this customer." });
                }

                _logger.LogInformation($"Payment retrieved successfully for Customer Id: {customerID}");
                return Ok(payments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during payment for customer with ID: {CustomerId}", customerID);
                return BadRequest(new { error = ex.Message });
            } 
            
        }


        [HttpGet("receipt/{paymentId}")]
        public async Task<IActionResult> GetReceipt(int paymentId)
        {
            _logger.LogInformation("Get receipts for customer with Payment ID: {PaymentId}", paymentId);

            try
            {
                var receipt = await _paymentService.GenerateReceiptAsync(paymentId);
                if (receipt == null)
                {
                    _logger.LogWarning("Receipt for PaymentID: {PaymentId} not found", paymentId);
                    return NotFound(new { message = "Receipt not found." });
                }

                _logger.LogInformation($"Receipt found successfully for Payment Id: {paymentId}");
                return Ok(receipt);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during getting receipt with PaymentID: {PaymentId}", paymentId);
                return BadRequest(new { error = ex.Message });
            }
        }

    }
}
