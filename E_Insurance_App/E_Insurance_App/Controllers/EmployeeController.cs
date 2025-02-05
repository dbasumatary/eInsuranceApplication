using E_Insurance_App.Models.DTOs;
using E_Insurance_App.Models.Entities;
using E_Insurance_App.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Insurance_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<EmployeeController> _logger; 

        public EmployeeController(IEmployeeService employeeService, ILogger<EmployeeController> logger)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        [HttpPost("register")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterEmployee(EmployeeRegisterDTO employeeDto)
        {
            _logger.LogInformation("Registering a new employee");

            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var employee = await _employeeService.RegisterEmployeeAsync(employeeDto);

                _logger.LogInformation($"Employee registered successfully for  EmployeeName: {employee.FullName}");
                return Ok(new { Message = "Employee registered successfully", Employee = employee });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering employee");
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpGet("{employeeId}")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> GetEmployeeById(int employeeId)
        {
            _logger.LogInformation("Retrieve employee with ID: {EmployeeId}", employeeId);

            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var employee = await _employeeService.GetEmployeeByIdAsync(employeeId);
                if (employee == null)
                {
                    _logger.LogWarning("Employee with ID: {EmployeeId} not found", employeeId);
                    return NotFound(new { Message = "Employee not found" });
                }

                _logger.LogInformation($"Employee retrieved successfully for  EmployeeId: {employee.EmployeeID}");
                return Ok(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during retrieve employee with ID: {EmployeeId}", employeeId);
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllEmployees()
        {
            _logger.LogInformation("Retrieve all employees");

            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var employees = await _employeeService.GetAllEmployeesAsync();
                if (employees == null)
                {
                    _logger.LogWarning("Employees not found");
                    return NotFound(new { Message = "Employees not found" });
                }

                _logger.LogInformation($"Employees retrieved successfully");
                return Ok(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during retrieve employees");
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpPut("update/{employeeId}")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> UpdateEmployee(int employeeId, [FromBody] EmployeeUpdateDTO employeeDto)
        {
            _logger.LogInformation("Update employee with ID: {EmployeeId}", employeeId);

            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var updatedEmployee = await _employeeService.UpdateEmployeeAsync(employeeId, employeeDto);
                if (updatedEmployee == null)
                {
                    _logger.LogWarning("Employee with ID: {EmployeeId} not found for update", employeeId);
                    return NotFound(new { Message = "Employee not found" });
                }

                return Ok(new { Message = "Employee updated successfully", Employee = updatedEmployee });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during update employee with ID: {EmployeeId}", employeeId);
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpDelete("delete/{employeeId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEmployee(int employeeId)
        {
            _logger.LogInformation("Delete employee with ID: {EmployeeId}", employeeId);

            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var deleted = await _employeeService.DeleteEmployeeAsync(employeeId);
                if (!deleted)
                {
                    _logger.LogWarning("Employee with ID: {EmployeeId} not found for delete", employeeId);
                    return NotFound(new { Message = "Employee not found" });
                }

                _logger.LogInformation("Employee with ID: {EmployeeId} deleted successfully", employeeId);
                return Ok(new { Message = "Employee deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during deleting employee with ID: {EmployeeId}", employeeId);
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
