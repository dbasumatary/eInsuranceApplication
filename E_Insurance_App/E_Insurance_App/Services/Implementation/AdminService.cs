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

        public AdminService(IAdminRepository adminRepository, IPasswordHasher passwordHasher)
        {
            _adminRepository = adminRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<Admin> RegisterAdminAsync(AdminRegisterDTO adminDto)
        {
            var admin = new Admin
            {
                Username = adminDto.Username,
                PasswordHash = _passwordHasher.HashPassword(adminDto.Password),
                Email = adminDto.Email,
                FullName = adminDto.FullName
            };

            return await _adminRepository.RegisterAdminAsync(admin);
        }
    }
}
