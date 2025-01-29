using E_Insurance_App.Data;
using E_Insurance_App.Models.Entities;
using E_Insurance_App.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace E_Insurance_App.Repositories.Implementation
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly InsuranceDbContext _context;
        private readonly ILogger<EmployeeRepository> _logger;

        public EmployeeRepository(InsuranceDbContext context, ILogger<EmployeeRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Employee> RegisterEmployeeAsync(Employee employee)
        {
            try
            {
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Employee registered: {employee.Username}");
                return employee;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error registering employee: {ex.Message}");
                throw;
            }
            
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int employeeId)
        {
            try
            {
                return await _context.Employees.FindAsync(employeeId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting employee: {ex.Message}");
                throw;
            }           
        }


        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            try
            {
                return await _context.Employees.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting employees: {ex.Message}");
                throw;
            }
        }


        public async Task<Employee?> UpdateEmployeeAsync(Employee employee)
        {
            try
            {
                var existingEmployee = await _context.Employees.FindAsync(employee.EmployeeID);
                if (existingEmployee == null)
                {
                    return null;
                }

                existingEmployee.FullName = employee.FullName;
                existingEmployee.Email = employee.Email;
                existingEmployee.Role = employee.Role;

                await _context.SaveChangesAsync();
                _logger.LogInformation($"Employee updated: {employee.Username}");
                return existingEmployee;
            }
            catch (Exception x)
            {
                _logger.LogError($"Error updating employee: {x.Message}");
                throw;
            }
        }


        public async Task<bool> DeleteEmployeeAsync(int employeeId)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(employeeId);
                if (employee == null)
                {
                    return false;
                }

                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Employee deleted: {employee.Username}");
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"Error deleting employee: {e.Message}");
                throw;
            }
        }
    }
}
