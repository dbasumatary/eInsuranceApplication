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

        public PolicyRepository(InsuranceDbContext context)
        {
            _context = context;
        }


        public async Task<int> CreatePolicyAsync(PolicyCreateDTO policyDTO)
        {
            try
            {
                int policyID = 0;
                var connectionString = _context.Database.GetDbConnection().ConnectionString;
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new Exception("Database connection string is null or empty!");
                }

                using (var connection = new SqlConnection(connectionString))
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
    }
}

