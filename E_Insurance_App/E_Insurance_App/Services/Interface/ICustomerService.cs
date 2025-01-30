using E_Insurance_App.Models.DTOs;
using E_Insurance_App.Models.Entities;

namespace E_Insurance_App.Services.Interface
{
    public interface ICustomerService
    {
        Task<Customer> RegisterCustomerAsync(CustomerRegisterDTO customerDto);
        Task<Customer> GetCustomerByIdAsync(int customerId);
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task<Customer?> UpdateCustomerAsync(int customerId, CustomerUpdateDTO customerDto);
        Task<bool> DeleteCustomerAsync(int customerId);
    }
}
