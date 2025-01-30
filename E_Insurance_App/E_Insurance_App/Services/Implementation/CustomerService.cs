using E_Insurance_App.Models.DTOs;
using E_Insurance_App.Models.Entities;
using E_Insurance_App.Repositories.Interface;
using E_Insurance_App.Services.Interface;

namespace E_Insurance_App.Services.Implementation
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(ICustomerRepository customerRepository, ILogger<CustomerService> logger)
        {
            _customerRepository = customerRepository;
            _logger = logger;
        }

        public async Task<Customer> RegisterCustomerAsync(CustomerRegisterDTO customerDto)
        {
            try
            {
                var customer = new Customer
                {
                    Username = customerDto.Username,
                    FullName = customerDto.FullName,
                    Email = customerDto.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(customerDto.Password),
                    Phone = customerDto.Phone,
                    DateOfBirth = customerDto.DateOfBirth,
                    AgentID = customerDto.AgentID
                };

                return await _customerRepository.RegisterCustomerAsync(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error registering customer: {ex.Message}");
                throw new Exception($"Error registering customer: {ex.Message}");
            }
        }


        public async Task<Customer?> GetCustomerByIdAsync(int customerId)
        {
            try
            {
                return await _customerRepository.GetCustomerByIdAsync(customerId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting customer by Id: {ex.Message}");
                throw new Exception($"Error getting customer by Id: {ex.Message}");
            }
        }


        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            try
            {
                return await _customerRepository.GetAllCustomersAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting customers list: {ex.Message}");
                throw new Exception($"Error getting customers list: {ex.Message}");
            }
        }


        public async Task<Customer?> UpdateCustomerAsync(int customerId, CustomerUpdateDTO customerDto)
        {
            try
            {
                var customer = await _customerRepository.GetCustomerByIdAsync(customerId);
                if (customer == null)
                {
                    return null;
                }
                customer.Username = customerDto.Username;
                customer.FullName = customerDto.FullName;
                customer.Email = customerDto.Email;
                customer.Phone = customerDto.Phone;
                customer.AgentID = customerDto.AgentID;

                return await _customerRepository.UpdateCustomerAsync(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating customer: {ex.Message}");
                throw new Exception($"Error updating customer: {ex.Message}");
            }
        }


        public async Task<bool> DeleteCustomerAsync(int customerId)
        {
            try
            {
                return await _customerRepository.DeleteCustomerAsync(customerId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting customer: {ex.Message}");
                throw new Exception($"Error deleting customer: {ex.Message}");
            }
        }
    }
}
