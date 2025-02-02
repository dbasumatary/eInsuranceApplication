using E_Insurance_App.Data;
using E_Insurance_App.Models.DTOs;
using E_Insurance_App.Repositories.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace E_Insurance_App.Repositories.Implementation
{
    public class CommissionRepository : ICommissionRepository
    {
        private readonly InsuranceDbContext _context;
        private readonly string _connectionString;

        public CommissionRepository(InsuranceDbContext context)
        {
            _context = context;
            _connectionString = _context.Database.GetDbConnection().ConnectionString;
        }

        public async Task<List<CommissionResponseDTO>> CalculateAgentCommissionAsync(int agentID)
        {
            try
            {
                var commissions = new List<CommissionResponseDTO>();

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("CalculateAgentCommission", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@AgentID", agentID));

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                commissions.Add(new CommissionResponseDTO
                                {
                                    CommissionID = reader.GetInt32("CommissionID"),
                                    AgentID = reader.GetInt32("AgentID"),
                                    AgentName = reader.GetString("AgentName"),
                                    PolicyID = reader.GetInt32("PolicyID"),
                                    PremiumID = reader.GetInt32("PremiumID"),
                                    CommissionAmount = reader.GetDecimal("CommissionAmount"),
                                    CreatedAt = reader.GetDateTime("CreatedAt")
                                });
                            }
                        }
                    }
                }

                return commissions;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error calculating commission: {ex.Message}");
            }
            
        }
    }
}
