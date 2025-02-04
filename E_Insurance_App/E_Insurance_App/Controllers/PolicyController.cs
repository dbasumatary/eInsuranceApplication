﻿using E_Insurance_App.Models.DTOs;
using E_Insurance_App.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Insurance_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PolicyController : ControllerBase
    {
        private readonly IPolicyService _policyService;

        public PolicyController(IPolicyService policyService)
        {
            _policyService = policyService;
        }


        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreatePolicy([FromBody] PolicyCreateDTO policyDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdPolicy = await _policyService.CreatePolicyAsync(policyDTO);
                return Ok(new { message = "Policy created successfully", policy = createdPolicy });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            
        }


        [HttpGet("{customerID}")]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> GetPoliciesByCustomer(int customerID)
        {
            try
            {
                var policies = await _policyService.GetCustomerPoliciesAsync(customerID);

                if (policies == null || policies.Count == 0)
                    return NotFound(new { message = "No policies found for this customer." });

                return Ok(policies);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            
        }


        [HttpPost("purchase")]
        [Authorize(Roles = "Customer, Employee")]
        public async Task<IActionResult> PurchasePolicy([FromBody] PolicyPurchaseDTO policyDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _policyService.PurchasePolicyAsync(policyDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpGet("{agentId}/policies")]
        [Authorize(Roles = "Agent, Employee")]
        public async Task<IActionResult> GetAgentPolicies(int agentId)
        {
            try
            {
                var policies = await _policyService.GetAgentPoliciesAsync(agentId);
                if (!policies.Any()) return NotFound("No policies found for this agent.");
                return Ok(policies);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
