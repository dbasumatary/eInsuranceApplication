using E_Insurance_App.Data;
using E_Insurance_App.Models.Entities;
using E_Insurance_App.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace E_Insurance_App.Repositories.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly InsuranceDbContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(InsuranceDbContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            try
            {
                //  Admins 
                var admin = await _context.Admins
                    .FirstOrDefaultAsync(a => a.Username == username);
                if (admin != null)
                {
                    _logger.LogInformation("Admin found with username: {Username}", username);
                    return new User { UserID = admin.AdminID, Username = admin.Username, PasswordHash = admin.PasswordHash, Role = "Admin" };
                }

                //  Employees 
                var employee = await _context.Employees
                    .FirstOrDefaultAsync(e => e.Username == username);
                if (employee != null)
                {
                    _logger.LogInformation("Employee found with username: {Username}", username);
                    return new User { UserID = employee.EmployeeID, Username = employee.Username, PasswordHash = employee.PasswordHash, Role = employee.Role };
                }

                //  Customers 
                var customer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.Username == username);
                if (customer != null)
                {
                    _logger.LogInformation("Customer found with username: {Username}", username);
                    return new User { UserID = customer.CustomerID, Username = customer.Email, PasswordHash = customer.Password, Role = "Customer" };
                }

                //  Agents
                var agent = await _context.Agents
                    .FirstOrDefaultAsync(a => a.Username == username);
                if (agent != null)
                {
                    _logger.LogInformation("Agent found with username: {Username}", username);
                    return new User { UserID = agent.AgentID, Username = agent.Username, PasswordHash = agent.Password, Role = "Agent" };
                }

                _logger.LogWarning("User not found with username: {Username}", username);
                return null; // User not found
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during retrieving user with username: {Username}", username);
                throw new Exception($"Error retrieving user: {ex.Message}");
            }
        }
    }
}
