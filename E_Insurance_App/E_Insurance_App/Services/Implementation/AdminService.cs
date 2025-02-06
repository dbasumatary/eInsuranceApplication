using E_Insurance_App.EmailService.Interface;
using E_Insurance_App.Models.DTOs;
using E_Insurance_App.Models.Entities;
using E_Insurance_App.Repositories.Interface;
using E_Insurance_App.Services.Interface;
using E_Insurance_App.Utilities.Interface;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace E_Insurance_App.Services.Implementation
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<AdminService> _logger;
        private readonly IRabbitMqService _rabbitMqService;

        public AdminService(IAdminRepository adminRepository, IPasswordHasher passwordHasher, ILogger<AdminService> logger, IRabbitMqService rabbitMqService)
        {
            _adminRepository = adminRepository;
            _passwordHasher = passwordHasher;
            _logger = logger;
            _rabbitMqService = rabbitMqService;
        }

        public async Task<Admin> RegisterAdminAsync(AdminRegisterDTO adminDto)
        {
            _logger.LogInformation("Registering new admin with username: {Username}", adminDto.Username);

            try
            {
                var plainPassword = adminDto.Password;

                var admin = new Admin
                {
                    Username = adminDto.Username,
                    PasswordHash = _passwordHasher.HashPassword(adminDto.Password),
                    Email = adminDto.Email,
                    FullName = adminDto.FullName
                };

                var regAdmin = await _adminRepository.RegisterAdminAsync(admin);

                _logger.LogInformation("Admin registered successfully with ID: {AdminID}", regAdmin.AdminID);

                var emailMessage = new
                {
                    Email = regAdmin.Email,
                    Subject = "Welcome to Insurance Company - Here are your Account Credentials",
                    Body = $@"
                        <p>Dear {regAdmin.FullName},</p>
                        <p>Welcome to <strong>Insurance Company</strong>. Your Admin account has been successfully created.</p>
                        <p><strong>Login Credentials:</strong></p>
                        <ul>
                            <li><strong>Username:</strong> {regAdmin.Username}</li>
                            <li><strong>Password:</strong> {plainPassword}</li>
                        </ul>
                        <p>For security reasons, please change your password after logging in.</p>
                        <p>If you did not request this account, please contact our support team immediately.</p>
                        <p>Best regards,<br/><strong>Insurance Company Support Team</strong></p>
                    "
                };

                _rabbitMqService.SendMessage("emailQueue", JsonConvert.SerializeObject(emailMessage));

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
