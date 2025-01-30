using E_Insurance_App.Data;
using E_Insurance_App.Models.Entities;
using E_Insurance_App.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace E_Insurance_App.Repositories.Implementation
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly InsuranceDbContext _context;
        private readonly ILogger<CustomerRepository> _logger;

        public CustomerRepository(InsuranceDbContext context, ILogger<CustomerRepository> logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<Customer> RegisterCustomerAsync(Customer customer)
        {
            try
            {
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Customer registered: {customer.FullName}");
                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error registering customer: {ex.Message}");
                throw;
            }
        }


        public async Task<Customer> GetCustomerByIdAsync(int customerId)
        {
            try
            {
                return await _context.Customers.FindAsync(customerId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting customer by Id: {ex.Message}");
                throw;
            }
        }


        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            try
            {
                return await _context.Customers.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting all customer: {ex.Message}");
                throw;
            }
        }


        public async Task<Customer> UpdateCustomerAsync(Customer customer)
        {
            try
            {
                _context.Customers.Update(customer);
                await _context.SaveChangesAsync();
                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating customer: {ex.Message}");
                throw;
            }
        }


        public async Task<bool> DeleteCustomerAsync(int customerId)
        {
            try
            {
                var customer = await _context.Customers.FindAsync(customerId);
                if (customer == null) return false;

                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting customer: {ex.Message}");
                throw;
            }
        }
    }
}
