using E_Insurance_App.Data;
using E_Insurance_App.Models.Entities;
using E_Insurance_App.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace E_Insurance_App.Repositories.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly InsuranceDbContext _context;

        public UserRepository(InsuranceDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            //  Admins 
            var admin = await _context.Admins
                .FirstOrDefaultAsync(a => a.Username == username);
            if (admin != null)
                return new User { UserID = admin.AdminID, Username = admin.Username, PasswordHash = admin.PasswordHash, Role = "Admin" };

            //  Employees 
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Username == username);
            if (employee != null)
                return new User { UserID = employee.EmployeeID, Username = employee.Username, PasswordHash = employee.PasswordHash, Role = employee.Role };

            //  Customers 
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Username == username);
            if (customer != null)
                return new User { UserID = customer.CustomerID, Username = customer.Email, PasswordHash = customer.Password, Role = "Customer" };

            //  Agents
            var agent = await _context.Agents
                .FirstOrDefaultAsync(a => a.Username == username);
            if (agent != null)
                return new User { UserID = agent.AgentID, Username = agent.Username, PasswordHash = agent.Password, Role = "Agent" };

            return null; // User not found
        }
    }
}
