using E_Insurance_App.Models.DTOs;
using E_Insurance_App.Models.Entities;
using E_Insurance_App.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace E_Insurance_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PolicyController : ControllerBase
    {
        private readonly IPolicyService _policyService;
        private readonly ILogger<PolicyController> _logger;

        public PolicyController(IPolicyService policyService, ILogger<PolicyController> logger)
        {
            _policyService = policyService;
            _logger = logger;
        }


        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreatePolicy([FromBody] PolicyCreateDTO policyDTO)
        {
            _logger.LogInformation("Creating new policy.");

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdPolicy = await _policyService.CreatePolicyAsync(policyDTO);

                _logger.LogInformation("Policy created successfully for CustomerId:  {CustomerId}", policyDTO.CustomerID);
                return Ok(new { message = "Policy created successfully", policy = createdPolicy });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating policy.");
                return BadRequest(new { error = ex.Message });
            }
            
        }


        [HttpGet("{customerID}")]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> GetPoliciesByCustomer(int customerID)
        {
            _logger.LogInformation("Retrieve policy with CustomerID: {CustomerId}", customerID);

            try
            {
                var policies = await _policyService.GetCustomerPoliciesAsync(customerID);

                if (policies == null || policies.Count == 0)
                {
                    _logger.LogWarning("Policy for CustomerID: {CustomerId} not found", customerID);
                    return NotFound(new { message = "No policies found for this customer." });
                }

                _logger.LogInformation("Policy retreived successfully for CustomerId: {CustomerId}", customerID);
                return Ok(policies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during retrieve policy with ID: {CustomerId}", customerID);
                return BadRequest(new { error = ex.Message });
            }
            
        }


        [HttpPost("purchase")]
        [Authorize(Roles = "Customer, Employee")]
        public async Task<IActionResult> PurchasePolicy([FromBody] PolicyPurchaseDTO policyDto)
        {
            _logger.LogInformation("Purchase policy option");

            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _policyService.PurchasePolicyAsync(policyDto);

                _logger.LogInformation("Policy purchased successfully for CustomerId: {CustomerId}", policyDto.CustomerID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during purchasing policy for CustomerID: {CustomerId}", policyDto.CustomerID);
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpGet("{agentId}/policies")]
        [Authorize(Roles = "Agent, Employee")]
        public async Task<IActionResult> GetAgentPolicies(int agentId)
        {
            _logger.LogInformation("Retrieve policy with AgentID: {AgentId}", agentId);

            try
            {
                var policies = await _policyService.GetAgentPoliciesAsync(agentId);
                if (!policies.Any())
                {
                    _logger.LogWarning("Policy for AgentID: {AgentId} not found", agentId);
                    return NotFound("No policies found for this agent.");
                }

                _logger.LogInformation("Policies retrieved successfully for AgentId: {AgentId}", agentId);
                return Ok(policies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during retrieving policy for AgentId: {AgentId}", agentId);
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpPost("search")]
        [Authorize(Roles = "Admin, Employee, Agent, Customer")]
        public async Task<IActionResult> SearchPolicies([FromBody] PolicySearchDTO searchCriteria)
        {
            _logger.LogInformation("Search policy with different criteria");

            try
            {
                var policies = await _policyService.SearchPoliciesAsync(searchCriteria);

                if (policies == null || policies.Count == 0)
                {
                    _logger.LogWarning("Polices not found");
                    return NotFound(new { message = "No policies found matching the criteria." });
                }

                _logger.LogInformation("Policies retrieved successfully");
                return Ok(policies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during retrieving policies");
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpPost("cancel")]
        [Authorize(Roles = "Customer,Employee")]
        public async Task<IActionResult> CancelPolicy([FromBody] PolicyCancellationDTO request)
        {
            _logger.LogInformation("Cancel policy.");

            try
            {
                if (request.PolicyID <= 0 || string.IsNullOrEmpty(request.Reason))
                {
                    _logger.LogWarning(" Invalid Policy Id given");
                    return BadRequest(new { message = "Invalid policy ID or reason." });
                }

                bool isCancelled = await _policyService.CancelPolicyAsync(request.PolicyID, request.Reason, request.CancelledBy);

                if (!isCancelled)
                {
                    _logger.LogWarning("Polices cancellation failed or policy not found");
                    return NotFound(new { message = "Policy cancellation failed or policy not found." });
                }

                _logger.LogInformation("Policies cancelled successfully for PolicyID: {PolicyID}", request.PolicyID);
                return Ok(new { message = "Policy cancelled successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during cancelling policies for PolicyID: {PolicyID}", request.PolicyID);
                return BadRequest(new { error = ex.Message });
            }
        }

    }
}

