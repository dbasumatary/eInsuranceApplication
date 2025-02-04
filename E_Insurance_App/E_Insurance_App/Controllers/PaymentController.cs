﻿using E_Insurance_App.Models.DTOs;
using E_Insurance_App.Models.Entities;
using E_Insurance_App.Repositories.Interface;
using E_Insurance_App.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace E_Insurance_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }


        [HttpPost("process")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentDTO paymentDto)
        {
            try
            {                 
                var response = await _paymentService.ProcessPaymentAsync(paymentDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpGet("View")]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> GetPaymentsByCustomer(int customerID)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var payments = await _paymentService.GetPaymentsByCustomer(customerID);

                if (payments == null || payments.Count == 0)
                    return NotFound(new { message = "No payments found for this customer." });

                return Ok(payments);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            
        }


        [HttpGet("receipt/{paymentId}")]
        public async Task<IActionResult> GetReceipt(int paymentId)
        {
            try
            {
                var receipt = await _paymentService.GenerateReceiptAsync(paymentId);
                if (receipt == null)
                {
                    return NotFound(new { message = "Receipt not found." });
                }

                return Ok(receipt);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

    }
}
