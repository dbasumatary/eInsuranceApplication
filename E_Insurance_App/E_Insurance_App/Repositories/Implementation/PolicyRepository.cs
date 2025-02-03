using E_Insurance_App.Data;
using E_Insurance_App.Models.DTOs;
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

        public PolicyRepository(InsuranceDbContext context)
        {
            _context = context;
            _connectionString = _context.Database.GetDbConnection().ConnectionString;
        }


        public async Task<int> CreatePolicyAsync(PolicyCreateDTO policyDTO)
        {
            try
            {
                int policyID = 0;
                
                if (string.IsNullOrEmpty(_connectionString))
                {
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
                return policyID;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating Policy: {ex.Message}");
            }
            
        }


        public async Task<List<PolicyViewDTO>> GetPoliciesByCustomerIDAsync(int customerID)
        {
            try
            {
                var policies = new List<PolicyViewDTO>();

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "GetPoliciesForCustomer";
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
                                    DateIssued = reader.GetDateTime(5),
                                    MaturityPeriod = reader.GetInt32(6),
                                    PolicyLapseDate = reader.GetDateTime(7),
                                    Status = reader.GetString(8),
                                    CreatedAt = reader.GetDateTime(9),
                                });
                            }
                        }
                    }
                }

                return policies;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting Policies by CustomerID: {ex.Message}");
            }
            
        }



        public async Task<PolicyResponseDTO> GetPolicyByIdAsync(int policyID)
        {
            try
            {
                return await _context.Policies
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
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting Policy by Id: {ex.Message}");
            }
            
        }


        public async Task<PolicyResponseDTO> PurchasePolicyAsync(PolicyPurchaseDTO policyDto)
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
            return policyResponse;
        }
    }
}

