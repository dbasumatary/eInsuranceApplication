using E_Insurance_App.EmailService.Interface;
using E_Insurance_App.Models.DTOs;
using E_Insurance_App.Models.Entities;
using E_Insurance_App.Repositories.Interface;
using E_Insurance_App.Services.Interface;
using E_Insurance_App.Utilities.Interface;
using Newtonsoft.Json;

namespace E_Insurance_App.Services.Implementation
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger<CustomerService> _logger;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IRabbitMqService _rabbitMqService;

        public CustomerService(ICustomerRepository customerRepository, ILogger<CustomerService> logger, IPasswordHasher passwordHasher, IRabbitMqService rabbitMqService)
        {
            _customerRepository = customerRepository;
            _logger = logger;
            _passwordHasher = passwordHasher;
            _rabbitMqService = rabbitMqService;
        }

        public async Task<Customer> RegisterCustomerAsync(CustomerRegisterDTO customerDto)
        {
            _logger.LogInformation($"Registering new customer with Username: {customerDto.Username}");

            try
            {
                var plainPassword = customerDto.Password;

                var customer = new Customer
                {
                    Username = customerDto.Username,
                    FullName = customerDto.FullName,
                    Email = customerDto.Email,
                    Password = _passwordHasher.HashPassword(customerDto.Password),
                    Phone = customerDto.Phone,
                    DateOfBirth = customerDto.DateOfBirth,
                    AgentID = customerDto.AgentID
                };

                var result = await _customerRepository.RegisterCustomerAsync(customer);

                _logger.LogInformation($"Customer with Username: {customerDto.Username} successfully registered.");

                var emailMessage = new
                {
                    Email = result.Email,
                    Subject = "Welcome to Insurance Company - Here are your Account Credentials",
                    Body = $@"
                        <p>Dear {result.FullName},</p>
                        <p>Welcome to <strong>Insurance Company</strong>. Your Customer account has been successfully created.</p>
                        <p><strong>Login Credentials:</strong></p>
                        <ul>
                            <li><strong>Username:</strong> {result.Username}</li>
                            <li><strong>Password:</strong> {plainPassword}</li>
                        </ul>
                        <p>For security reasons, please change your password after logging in.</p>
                        <p>If you did not request this account, please contact our support team immediately.</p>
                        <p>Best regards,<br/><strong>Insurance Company Support Team</strong></p>
                    "
                };

                _rabbitMqService.SendMessage("emailQueue", JsonConvert.SerializeObject(emailMessage));

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during registering customer with Username: {customerDto.Username}, Error: {ex.Message}");
                throw new Exception($"Error registering customer: {ex.Message}");
            }
        }


        public async Task<Customer?> GetCustomerByIdAsync(int customerId)
        {
            _logger.LogInformation($"Retrieving customer details for CustomerID: {customerId}");

            try
            {
                var customer = await _customerRepository.GetCustomerByIdAsync(customerId);
                if (customer == null)
                {
                    _logger.LogWarning($"Customer with CustomerID: {customerId} not found.");
                }

                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during getting customer by Id: {customerId}, Error: {ex.Message}");
                throw new Exception($"Error getting customer by Id: {ex.Message}");
            }
        }


        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            _logger.LogInformation("Retrieving all customers.");

            try
            {
                var customers = await _customerRepository.GetAllCustomersAsync();

                _logger.LogInformation($"Retrieved {customers.Count()} customers.");
                return customers;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during retrieving customers list: {ex.Message}");
                throw new Exception($"Error retrieving customers list: {ex.Message}");
            }
        }


        public async Task<Customer?> UpdateCustomerAsync(int customerId, CustomerUpdateDTO customerDto)
        {
            _logger.LogInformation($"Updating customer details for CustomerID: {customerId}");

            try
            {
                var customer = await _customerRepository.GetCustomerByIdAsync(customerId);
                if (customer == null)
                {
                    _logger.LogWarning($"Customer with CustomerID: {customerId} not found.");
                    return null;
                }

                customer.Username = customerDto.Username;
                customer.FullName = customerDto.FullName;
                customer.Email = customerDto.Email;
                customer.Phone = customerDto.Phone;
                customer.AgentID = customerDto.AgentID;

                var updatedCustomer = await _customerRepository.UpdateCustomerAsync(customer);

                _logger.LogInformation($"Customer with CustomerID: {customerId} successfully updated.");
                return updatedCustomer;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during updating customer with CustomerID: {customerId}, Error: {ex.Message}");
                throw new Exception($"Error updating customer: {ex.Message}");
            }
        }


        public async Task<bool> DeleteCustomerAsync(int customerId)
        {
            _logger.LogInformation($"Delete customer with CustomerID: {customerId}");

            try
            {
                var result = await _customerRepository.DeleteCustomerAsync(customerId);
                if (result)
                {
                    _logger.LogInformation($"Customer with CustomerID: {customerId} successfully deleted.");
                }
                else
                {
                    _logger.LogWarning($"Customer with CustomerID: {customerId} not found for deletion.");
                }

                return result;
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error during deleting customer with CustomerID: {customerId}, Error: {ex.Message}");
                throw new Exception($"Error deleting customer: {ex.Message}");
            }
        }
    }
}

