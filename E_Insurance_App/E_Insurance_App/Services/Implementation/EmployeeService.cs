﻿using E_Insurance_App.EmailService.Interface;
using E_Insurance_App.Models.DTOs;
using E_Insurance_App.Models.Entities;
using E_Insurance_App.Repositories.Interface;
using E_Insurance_App.Services.Interface;
using E_Insurance_App.Utilities.Interface;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace E_Insurance_App.Services.Implementation
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<EmployeeService> _logger;
        private readonly IRabbitMqService _rabbitMqService;

        public EmployeeService(IEmployeeRepository employeeRepository, IPasswordHasher passwordHasher, ILogger<EmployeeService> logger, IRabbitMqService rabbitMqService)
        {
            _employeeRepository = employeeRepository;
            _passwordHasher = passwordHasher;
            _logger = logger;
            _rabbitMqService = rabbitMqService;
        }

        public async Task<Employee> RegisterEmployeeAsync(EmployeeRegisterDTO employeeDto)
        {
            _logger.LogInformation($"Registering new employee with Username: {employeeDto.Username}");

            try
            {
                var plainPassword = employeeDto.Password;

                var employee = new Employee
                {
                    Username = employeeDto.Username,
                    PasswordHash = _passwordHasher.HashPassword(employeeDto.Password),
                    Email = employeeDto.Email,
                    FullName = employeeDto.FullName
                };
                var result = await _employeeRepository.RegisterEmployeeAsync(employee);

                _logger.LogInformation($"Employee with Username: {employeeDto.Username} successfully registered.");

                var emailMessage = new
                {
                    Email = result.Email,
                    Subject = "Welcome to Insurance Company - Here are your Account Credentials",
                    Body = $@"
                        <p>Dear {result.FullName},</p>
                        <p>Welcome to <strong>Insurance Company</strong>. Your Employee account has been successfully created.</p>
                        <p><strong>Login Credentials:</strong></p>
                        <ul>
                            <li><strong>Username:</strong> {result.Username}</li>
                            <li><strong>Password:</strong> {plainPassword}</li>
                        </ul>
                        <p>For security reasons, please change your password after logging in.</p>
                        <p>If you did not request this account, please contact our support team immediately.</p>
                        <p>Best regards,<br/><strong>Insurance Company Support Team</strong></p>
                    "
                };

                _rabbitMqService.SendMessage("emailQueue", JsonConvert.SerializeObject(emailMessage));

                return result;
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error during registering employee with Username: {employeeDto.Username}, Error: {ex.Message}");
                throw new Exception($"Error registering employee: {ex.Message}");
            }
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int employeeId)
        {
            _logger.LogInformation($"Retrieving employee details for EmployeeID: {employeeId}");

            try
            {
                var employee = await _employeeRepository.GetEmployeeByIdAsync(employeeId);

                if (employee == null)
                {
                    _logger.LogWarning($"Employee with EmployeeID: {employeeId} not found.");
                }

                return employee;
            }
            catch (Exception x)
            {
                _logger.LogError($"Error during getting employee by Id: {employeeId}, Error: {x.Message}");
                throw new Exception($"Error getting employee: {x.Message}");
            }
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            _logger.LogInformation("Retrieving all employees.");

            try
            {
                var employees = await _employeeRepository.GetAllEmployeesAsync();

                _logger.LogInformation($"Retrieved {employees.Count()} employees.");
                return employees;
            }
            catch (Exception x)
            {
                _logger.LogError($"Error during retrieving employees list: {x.Message}");
                throw new Exception($"Error retrieving employees: {x.Message}");
            }

        }

        public async Task<Employee?> UpdateEmployeeAsync(int employeeId, EmployeeUpdateDTO employeeDto)
        {
            _logger.LogInformation($"Updating employee details for EmployeeID: {employeeId}");

            try
            {
                var employee = await _employeeRepository.GetEmployeeByIdAsync(employeeId);
                if (employee == null)
                {
                    _logger.LogWarning($"Employee with EmployeeID: {employeeId} not found.");
                    return null;
                }

                employee.FullName = employeeDto.FullName;
                employee.Email = employeeDto.Email;
                employee.Role = employeeDto.Role;

                var updatedEmployee = await _employeeRepository.UpdateEmployeeAsync(employee);

                _logger.LogInformation($"Employee with EmployeeID: {employeeId} successfully updated.");
                return updatedEmployee;
            }
            catch (Exception x)
            {
                _logger.LogError($"Error during updating employee with EmployeeID: {employeeId}, Error: {x.Message}");
                throw new Exception($"Error updating employees: {x.Message}");
            }
        }

        public async Task<bool> DeleteEmployeeAsync(int employeeId)
        {
            _logger.LogInformation($"Delete employee with EmployeeID: {employeeId}");

            try
            {
                var result = await _employeeRepository.DeleteEmployeeAsync(employeeId);

                if (result)
                {
                    _logger.LogInformation($"Employee with EmployeeID: {employeeId} successfully deleted.");
                }
                else
                {
                    _logger.LogWarning($"Employee with EmployeeID: {employeeId} not found for deletion.");
                }

                return result;
            }
            catch (Exception x)
            {
                _logger.LogError($"Error during deleting employee with EmployeeID: {employeeId}, Error: {x.Message}");

                throw new Exception($"Error getting employees: {x.Message}");
            }
        }
    }
}
