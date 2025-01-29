using E_Insurance_App.Models.DTOs;
using E_Insurance_App.Models.Entities;

namespace E_Insurance_App.Services.Interface
{
    public interface IEmployeeService
    {
        Task<Employee> RegisterEmployeeAsync(EmployeeRegisterDTO employeeDto);
        Task<Employee?> GetEmployeeByIdAsync(int employeeId);
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<Employee?> UpdateEmployeeAsync(int employeeId, EmployeeUpdateDTO employeeDto);
        Task<bool> DeleteEmployeeAsync(int employeeId);
    }
}
