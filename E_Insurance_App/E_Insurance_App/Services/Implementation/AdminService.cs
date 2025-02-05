using E_Insurance_App.Models.DTOs;
using E_Insurance_App.Models.Entities;
using E_Insurance_App.Repositories.Interface;
using E_Insurance_App.Services.Interface;
using E_Insurance_App.Utilities.Interface;
using Microsoft.AspNetCore.Identity;

namespace E_Insurance_App.Services.Implementation
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<AdminService> _logger;


        public AdminService(IAdminRepository adminRepository, IPasswordHasher passwordHasher, ILogger<AdminService> logger)
        {
            _adminRepository = adminRepository;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task<Admin> RegisterAdminAsync(AdminRegisterDTO adminDto)
        {
            _logger.LogInformation("Registering new admin with username: {Username}", adminDto.Username);

            try
            {
                var admin = new Admin
                {
                    Username = adminDto.Username,
                    PasswordHash = _passwordHasher.HashPassword(adminDto.Password),
                    Email = adminDto.Email,
                    FullName = adminDto.FullName
                };

                var regAdmin = await _adminRepository.RegisterAdminAsync(admin);

                _logger.LogInformation("Admin registered successfully with ID: {AdminID}", regAdmin.AdminID);
                return regAdmin;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during register admin with username: {Username}", adminDto.Username);
                throw new Exception($"Error registering admin: {ex.Message}");
            }
            
        }
    }
}
