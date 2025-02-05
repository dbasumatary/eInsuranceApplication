using E_Insurance_App.Data;
using E_Insurance_App.Models.DTOs;
using E_Insurance_App.Models.Entities;
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
        private readonly ILogger<CommissionRepository> _logger;


        public CommissionRepository(InsuranceDbContext context, ILogger<CommissionRepository> logger)
        {
            _context = context;
            _connectionString = _context.Database.GetDbConnection().ConnectionString;
            _logger = logger;
        }

        public async Task<List<CommissionResponseDTO>> CalculateAgentCommissionAsync(int agentID)
        {
            _logger.LogInformation("Commission calculation for AgentID: {AgentID}", agentID);

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

                _logger.LogInformation("Successfully calculated commissions for AgentID: {AgentID}.", agentID);
                return commissions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating commission for AgentID: {AgentID}", agentID);
                throw new Exception($"Error calculating commission: {ex.Message}");
            }
            
        }


        public async Task<List<CommissionResponseDTO>> GetAgentCommissionsAsync(int agentId)
        {
            _logger.LogInformation("Fetching commissions for AgentID: {AgentID}", agentId);

            try
            {
                var commissions = new List<CommissionResponseDTO>();

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("GetAgentCommission", connection))
                    {
                        //command.CommandText = "GetAgentCommission";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@AgentID", agentId));

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                commissions.Add(new CommissionResponseDTO
                                {
                                    CommissionID = reader.GetInt32(0),
                                    AgentID = reader.GetInt32(1),
                                    AgentName = reader.GetString(2),
                                    PolicyID = reader.GetInt32(3),
                                    PremiumID = reader.GetInt32(4),
                                    CommissionAmount = reader.GetDecimal(5),
                                    CreatedAt = reader.GetDateTime(6),
                                    IsPaid = reader.GetBoolean(7),
                                    PaymentProcessedDate = reader.IsDBNull(reader.GetOrdinal("PaymentProcessedDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("PaymentProcessedDate"))
                                });
                            }
                        }
                    }
                }

                _logger.LogInformation("Successfully found commissions for AgentID: {AgentID}.", agentId);
                return commissions;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during retrieve commissions for AgentID: {AgentID}", agentId);
                throw new Exception($"Error during retrieve commissions: {ex.Message}");
            }
        }

        public async Task<List<Commission>> GetCommissionsByAgentIdAsync(int agentId)
        {
            _logger.LogInformation("Getting commissions from DbContext for AgentID: {AgentID}", agentId);

            try
            {
                var commissions = await _context.Commissions
                                     .Where(c => c.AgentID == agentId)
                                     .ToListAsync();

                _logger.LogInformation("Successfully retrieved commissions for AgentID: {AgentID}.", agentId);

                return commissions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during retrieve commissions for AgentID: {AgentID}", agentId);
                throw new Exception($"Error during retrieve commissions: {ex.Message}");
            }
        }


       
        public async Task PayCommissionsAsync(List<Commission> commissions)
        {
            _logger.LogInformation("Starting payment processing for {CommissionCount} commissions.", commissions.Count);
          
            try
            {
                _context.Commissions.UpdateRange(commissions);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully processed payment for {CommissionCount} commissions.", commissions.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment for commissions.");
                throw new Exception($"Error processing payment: {ex.Message}");
            }
        }
    }
}

