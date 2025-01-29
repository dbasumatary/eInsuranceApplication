using E_Insurance_App.Models.DTOs;
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

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost("register")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterEmployee(EmployeeRegisterDTO employeeDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var employee = await _employeeService.RegisterEmployeeAsync(employeeDto);
                return Ok(new { Message = "Employee registered successfully", Employee = employee });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpGet("{employeeId}")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> GetEmployeeById(int employeeId)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var employee = await _employeeService.GetEmployeeByIdAsync(employeeId);
                if (employee == null)
                    return NotFound(new { Message = "Employee not found" });

                return Ok(employee);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var employees = await _employeeService.GetAllEmployeesAsync();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpPut("update/{employeeId}")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> UpdateEmployee(int employeeId, [FromBody] EmployeeUpdateDTO employeeDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var updatedEmployee = await _employeeService.UpdateEmployeeAsync(employeeId, employeeDto);
                if (updatedEmployee == null)
                    return NotFound(new { Message = "Employee not found" });

                return Ok(new { Message = "Employee updated successfully", Employee = updatedEmployee });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpDelete("delete/{employeeId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEmployee(int employeeId)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var deleted = await _employeeService.DeleteEmployeeAsync(employeeId);
                if (!deleted)
                    return NotFound(new { Message = "Employee not found" });

                return Ok(new { Message = "Employee deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
