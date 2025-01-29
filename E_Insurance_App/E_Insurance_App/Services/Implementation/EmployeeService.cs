using E_Insurance_App.Models.DTOs;
using E_Insurance_App.Models.Entities;
using E_Insurance_App.Repositories.Interface;
using E_Insurance_App.Services.Interface;
using E_Insurance_App.Utilities.Interface;
using Microsoft.AspNetCore.Identity;

namespace E_Insurance_App.Services.Implementation
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPasswordHasher _passwordHasher;

        public EmployeeService(IEmployeeRepository employeeRepository, IPasswordHasher passwordHasher)
        {
            _employeeRepository = employeeRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<Employee> RegisterEmployeeAsync(EmployeeRegisterDTO employeeDto)
        {
            try
            {
                var employee = new Employee
                {
                    Username = employeeDto.Username,
                    PasswordHash = _passwordHasher.HashPassword(employeeDto.Password),
                    Email = employeeDto.Email,
                    FullName = employeeDto.FullName
                };

                return await _employeeRepository.RegisterEmployeeAsync(employee);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error registering employee: {ex.Message}");
            }
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int employeeId)
        {
            try
            {
                return await _employeeRepository.GetEmployeeByIdAsync(employeeId);
            }
            catch (Exception x)
            {
                throw new Exception($"Error getting employee: {x.Message}");
            }
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            try
            {
                return await _employeeRepository.GetAllEmployeesAsync();
            }
            catch (Exception x)
            {
                throw new Exception($"Error getting employees: {x.Message}");
            }
            
        }

        public async Task<Employee?> UpdateEmployeeAsync(int employeeId, EmployeeUpdateDTO employeeDto)
        {
            try
            {
                var employee = await _employeeRepository.GetEmployeeByIdAsync(employeeId);
                if (employee == null)
                {
                    return null;
                }

                employee.FullName = employeeDto.FullName;
                employee.Email = employeeDto.Email;
                employee.Role = employeeDto.Role;

                return await _employeeRepository.UpdateEmployeeAsync(employee);
            }
            catch (Exception x)
            {
                throw new Exception($"Error updating employees: {x.Message}");
            }           
        }

        public async Task<bool> DeleteEmployeeAsync(int employeeId)
        {
            try
            {
                return await _employeeRepository.DeleteEmployeeAsync(employeeId);
            }
            catch (Exception x)
            {
                throw new Exception($"Error getting employees: {x.Message}");
            }
        }
    }
}
