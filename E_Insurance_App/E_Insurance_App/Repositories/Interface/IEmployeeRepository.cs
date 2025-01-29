using E_Insurance_App.Models.Entities;

namespace E_Insurance_App.Repositories.Interface
{
    public interface IEmployeeRepository
    {
        Task<Employee> RegisterEmployeeAsync(Employee employee);
        Task<Employee?> GetEmployeeByIdAsync(int employeeId);
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<Employee?> UpdateEmployeeAsync(Employee employee);
        Task<bool> DeleteEmployeeAsync(int employeeId);
    }
}
