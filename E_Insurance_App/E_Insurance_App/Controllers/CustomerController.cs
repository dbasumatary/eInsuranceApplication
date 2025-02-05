using E_Insurance_App.Models.DTOs;
using E_Insurance_App.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Insurance_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ICustomerService customerService, ILogger<CustomerController> logger)
        {
            _customerService = customerService;
            _logger = logger;
        }


        [HttpPost("register")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterCustomer(CustomerRegisterDTO customerDto)
        {
            _logger.LogInformation("Registering a new customer");

            try
            {
                var customer = await _customerService.RegisterCustomerAsync(customerDto);

                _logger.LogInformation("Customer registered successfully for {CustomerName}", customer.FullName);
                return Ok(new { Message = "Customer registered successfully", Customer = customer });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering customer");
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            _logger.LogInformation("Retrieve customer with ID: {CustomerId}", id);

            try
            {
                var customer = await _customerService.GetCustomerByIdAsync(id);
                if (customer == null)
                {
                    _logger.LogWarning("Customer with ID: {CustomerId} not found", id);
                    return NotFound(new { Message = "Customer not found" });
                }

                _logger.LogInformation("Customer retreived for Id: {CustomerId}", id);
                return Ok(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during retrieve customer with ID: {CustomerId}", id);
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpGet("all")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> GetAllCustomers()
        {
            _logger.LogInformation("Retrieve all customers");

            try
            {
                var customers = await _customerService.GetAllCustomersAsync();

                _logger.LogInformation("Customers retreived");
                return Ok(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during retrieve customers");
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpPut("update/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCustomer(int id, CustomerUpdateDTO customerDto)
        {
            _logger.LogInformation("Updating customer with ID: {CustomerId}", id);

            try
            {
                var updatedCustomer = await _customerService.UpdateCustomerAsync(id, customerDto);
                if (updatedCustomer == null)
                {
                    _logger.LogWarning("Customer with ID: {CustomerId} not found for update", id);
                    return NotFound(new { Message = "Customer not found" });
                }

                _logger.LogInformation("Customer with ID: {CustomerId} updated successfully", id);
                return Ok(new { Message = "Customer updated successfully", Customer = updatedCustomer });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during update customer with ID: {CustomerId}", id);
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            _logger.LogInformation("Deleting customer with ID: {CustomerId}", id);

            try
            {
                var deleted = await _customerService.DeleteCustomerAsync(id);
                if (!deleted)
                {
                    _logger.LogWarning("Customer with ID: {CustomerId} not found for deletion", id);
                    return NotFound(new { Message = "Customer not found" });
                }

                _logger.LogInformation("Customer with ID: {CustomerId} deleted successfully", id);
                return Ok(new { Message = "Customer deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during delete customer with ID: {CustomerId}", id);
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
