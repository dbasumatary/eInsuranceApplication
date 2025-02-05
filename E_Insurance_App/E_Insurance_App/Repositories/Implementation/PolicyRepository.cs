using E_Insurance_App.Data;
using E_Insurance_App.Models.DTOs;
using E_Insurance_App.Models.Entities;
using E_Insurance_App.Repositories.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace E_Insurance_App.Repositories.Implementation
{
    public class PolicyRepository : IPolicyRepository
    {
        private readonly InsuranceDbContext _context;
        private readonly string _connectionString;
        private readonly ILogger<PolicyRepository> _logger;

        public PolicyRepository(InsuranceDbContext context, ILogger<PolicyRepository> logger)
        {
            _context = context;
            _connectionString = _context.Database.GetDbConnection().ConnectionString;
            _logger = logger;
        }


        public async Task<int> CreatePolicyAsync(PolicyCreateDTO policyDTO)
        {
            _logger.LogInformation("Creating policy for CustomerID: {CustomerID}, SchemeID: {SchemeID}", policyDTO.CustomerID, policyDTO.SchemeID);

            try
            {
                int policyID = 0;
                
                if (string.IsNullOrEmpty(_connectionString))
                {
                    _logger.LogError("Database connection string is null or empty!");
                    throw new Exception("Database connection string is null or empty!");
                }

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("ValidatePolicy", connection))
                    {
                        //command.CommandText = "ValidatePolicy";
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@CustomerID", policyDTO.CustomerID));
                        command.Parameters.Add(new SqlParameter("@SchemeID", policyDTO.SchemeID));
                        command.Parameters.Add(new SqlParameter("@PolicyDetails", policyDTO.PolicyDetails));
                        command.Parameters.Add(new SqlParameter("@DateIssued", policyDTO.DateIssued));
                        command.Parameters.Add(new SqlParameter("@MaturityPeriod", policyDTO.MaturityPeriod));
                        command.Parameters.Add(new SqlParameter("@PolicyLapseDate", policyDTO.PolicyLapseDate));

                        var outputParam = new SqlParameter("@PolicyID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outputParam);

                        await command.ExecuteNonQueryAsync();

                        policyID = (int)outputParam.Value;
                    }
                }

                _logger.LogInformation("Policy created successfully with PolicyID: {PolicyID}", policyID);
                return policyID;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during creating policy.");
                throw new Exception($"Error creating Policy: {ex.Message}");
            }
            
        }


        public async Task<List<PolicyViewDTO>> GetPoliciesByCustomerIDAsync(int customerID)
        {
            _logger.LogInformation("Retrieving policies for CustomerID: {CustomerID}", customerID);

            try
            {
                var policies = new List<PolicyViewDTO>();

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("GetPoliciesForCustomer", connection))
                    {
                        //command.CommandText = "GetPoliciesForCustomer";
                        command.CommandType = CommandType.StoredProcedure;

                        var param = command.CreateParameter();
                        param.ParameterName = "@CustomerID";
                        param.Value = customerID;
                        command.Parameters.Add(param);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                policies.Add(new PolicyViewDTO
                                {
                                    PolicyID = reader.GetInt32(0),
                                    CustomerID = reader.GetInt32(1),
                                    CustomerName = reader.GetString(2),
                                    SchemeID = reader.GetInt32(3),
                                    PolicyDetails = reader.GetString(4),
                                    Premium = reader.IsDBNull(5) ? 0 : reader.GetDecimal(5),
                                    DateIssued = reader.GetDateTime(6),
                                    MaturityPeriod = reader.GetInt32(7),
                                    PolicyLapseDate = reader.GetDateTime(8),
                                    Status = reader.GetString(9),
                                    CreatedAt = reader.GetDateTime(10),
                                });
                            }
                        }
                    }
                }

                _logger.LogInformation("Retrieved the policy for CustomerID: {CustomerID}", customerID);
                return policies;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during retrieving policy by CustomerID: {CustomerID}", customerID);
                throw new Exception($"Error getting Policies by CustomerID: {ex.Message}");
            }
            
        }



        public async Task<PolicyResponseDTO> GetPolicyByIdAsync(int policyID)
        {
            _logger.LogInformation("Retrieving policy with PolicyID: {PolicyID}", policyID);

            try
            {
                var policy = await _context.Policies
                    .Where(p => p.PolicyID == policyID)
                    .Select(p => new PolicyResponseDTO
                    {
                        PolicyID = p.PolicyID,
                        CustomerID = p.CustomerID,
                        SchemeID = p.SchemeID,
                        PolicyDetails = p.PolicyDetails,
                        DateIssued = p.DateIssued,
                        MaturityPeriod = p.MaturityPeriod,
                        PolicyLapseDate = p.PolicyLapseDate,
                        Status = p.Status
                    })
                    .FirstOrDefaultAsync();

                if (policy == null)
                {
                    _logger.LogWarning("No policy found with PolicyID: {PolicyID}", policyID);
                }

                return policy;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during retrieving policy with PolicyID: {PolicyID}", policyID);
                throw new Exception($"Error getting Policy by Id: {ex.Message}");
            }
            
        }


        public async Task<PolicyResponseDTO> PurchasePolicyAsync(PolicyPurchaseDTO policyDto)
        {
            _logger.LogInformation("Purchasing policy for CustomerID: {CustomerID}, SchemeID: {SchemeID}", policyDto.CustomerID, policyDto.SchemeID);

            try
            {
                var policyResponse = new PolicyResponseDTO();

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("PurchasePolicy", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@CustomerID", policyDto.CustomerID));
                        command.Parameters.Add(new SqlParameter("@SchemeID", policyDto.SchemeID));
                        command.Parameters.Add(new SqlParameter("@PolicyDetails", policyDto.PolicyDetails));
                        command.Parameters.Add(new SqlParameter("@BaseRate", policyDto.BaseRate));
                        command.Parameters.Add(new SqlParameter("@MaturityPeriod", policyDto.MaturityPeriod));

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                policyResponse.PolicyID = reader.GetInt32(0);
                                policyResponse.CustomerID = reader.GetInt32(1);
                                policyResponse.SchemeID = reader.GetInt32(2);
                                policyResponse.PolicyDetails = reader.GetString(3);
                                policyResponse.CalculatedPremium = reader.GetDecimal(4);
                                policyResponse.DateIssued = reader.GetDateTime(5);
                                policyResponse.MaturityPeriod = reader.GetInt32(6);
                                policyResponse.PolicyLapseDate = reader.GetDateTime(7);
                                policyResponse.Status = reader.GetString(8);
                            }
                        }
                    }
                }

                _logger.LogInformation("Policy purchased successfully with PolicyID: {PolicyID}", policyResponse.PolicyID);
                return policyResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during policy purchase");
                throw new Exception($"Error getting Policy by Id: {ex.Message}");
            }
        }


        public async Task<IEnumerable<PolicyResponseDTO>> GetAgentPoliciesAsync(int agentId)
        {
            _logger.LogInformation("Retrieving policies for AgentID: {AgentID}", agentId);

            try
            {
                var policies = await _context.Policies
                    .Where(p => p.Customer.AgentID == agentId)
                    .Select(p => new PolicyResponseDTO
                    {
                        PolicyID = p.PolicyID,
                        CustomerID = p.CustomerID,
                        SchemeID = p.SchemeID,
                        PolicyDetails = p.PolicyDetails,
                        CalculatedPremium = p.Premiums.FirstOrDefault().CalculatedPremium,
                        DateIssued = p.DateIssued,
                        MaturityPeriod = p.MaturityPeriod,
                        PolicyLapseDate = p.PolicyLapseDate,
                        Status = p.Status
                    })
                    .ToListAsync();

                _logger.LogInformation("Retrieved {PolicyCount} policies for AgentID: {AgentID}", policies.Count, agentId);
                return policies;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during retrieving policies for AgentID: {AgentID}", agentId);
                throw new Exception($"Error retrieving policies by AgentID: {ex.Message}");
            }
        }


        public async Task<List<PolicyResponseDTO>> SearchPoliciesAsync(PolicySearchDTO searchCriteria)
        {
            _logger.LogInformation("Searching policies with criteria: {SearchCriteria}", searchCriteria);

            try
            {
                var policies = new List<PolicyResponseDTO>();

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("SearchPolicies", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@PolicyId", searchCriteria.PolicyId ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@CustomerName", searchCriteria.CustomerName ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@AgentID", searchCriteria.AgentID ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@EmployeeID", searchCriteria.EmployeeID ?? (object)DBNull.Value);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                policies.Add(new PolicyResponseDTO
                                {
                                    PolicyID = reader.GetInt32(reader.GetOrdinal("PolicyID")),
                                    CustomerID = reader.GetInt32(reader.GetOrdinal("CustomerID")),
                                    SchemeID = reader.GetInt32(reader.GetOrdinal("SchemeID")),
                                    PolicyDetails = reader.GetString(reader.GetOrdinal("PolicyDetails")),
                                    CalculatedPremium = reader.GetDecimal(reader.GetOrdinal("CalculatedPremium")),
                                    DateIssued = reader.GetDateTime(reader.GetOrdinal("DateIssued")),
                                    MaturityPeriod = reader.GetInt32(reader.GetOrdinal("MaturityPeriod")),
                                    PolicyLapseDate = reader.GetDateTime(reader.GetOrdinal("PolicyLapseDate")),
                                    Status = reader.GetString(reader.GetOrdinal("Status"))
                                });
                            }
                        }
                    }
                }

                _logger.LogInformation("Found {PolicyCount} policies matching search criteria.", policies.Count);
                return policies;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during searching policies with criteria: {SearchCriteria}", searchCriteria);
                throw new Exception($"Error searching Policy details: {ex.Message}");
            }
        }


        public async Task<bool> CancelPolicyAsync(int policyId, string reason, string cancelledBy)
        {
            _logger.LogInformation("Cancelling policy");

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("CancelPolicy", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@PolicyID", policyId);
                        command.Parameters.AddWithValue("@Reason", reason);
                        command.Parameters.AddWithValue("@CancelledBy", cancelledBy);

                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        _logger.LogInformation("The policy is cancelled");
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during cancelling policy");
                throw new Exception($"Error cancelling policy: {ex.Message}");
            }
        }

    }
}

