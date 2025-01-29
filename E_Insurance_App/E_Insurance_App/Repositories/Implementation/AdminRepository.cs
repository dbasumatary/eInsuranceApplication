using E_Insurance_App.Data;
using E_Insurance_App.Models.Entities;
using E_Insurance_App.Repositories.Interface;

namespace E_Insurance_App.Repositories.Implementation
{
    public class AdminRepository : IAdminRepository
    {
        private readonly InsuranceDbContext _context;
        private readonly ILogger<AdminRepository> _logger;

        public AdminRepository(InsuranceDbContext context, ILogger<AdminRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Admin> RegisterAdminAsync(Admin admin)
        {
            try
            {
                _context.Admins.Add(admin);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Admin registered: {admin.Username}");
                return admin;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error registering admin: {ex.Message}");
                throw;
            }
            
        }
    }
}
