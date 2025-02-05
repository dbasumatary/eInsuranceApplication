using E_Insurance_App.Data;
using E_Insurance_App.Models.DTOs;
using E_Insurance_App.Models.Entities;
using E_Insurance_App.Repositories.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace E_Insurance_App.Repositories.Implementation
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly InsuranceDbContext _context;
        private readonly string _connectionString;
        private readonly ILogger<PaymentRepository> _logger;

        public PaymentRepository(InsuranceDbContext context, ILogger<PaymentRepository> logger)
        {
            _context = context;
            _connectionString = _context.Database.GetDbConnection().ConnectionString;
            _logger = logger;
        }

        public async Task<PaymentResponseDTO> ProcessPaymentAsync(Payment request)
        {
            _logger.LogInformation("Processing payment for CustomerID: {CustomerID}, PolicyID: {PolicyID}, PremiumID: {PremiumID}",
                request.CustomerID, request.PolicyID, request.PremiumID);

            try
            {
                var connectionString = _context.Database.GetDbConnection().ConnectionString;
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new Exception("Database connection string is null or empty!");
                }

                var premium = await _context.Premiums.FindAsync(request.PremiumID);
                if (premium == null)
                {
                    _logger.LogWarning("Invalid PremiumID: {PremiumID}", request.PremiumID);
                    throw new Exception("Invalid PremiumID");
                }


                // Calculate
                if (request.PaymentType == "Monthly")
                {
                    request.Amount = premium.CalculatedPremium / 12;
                }   
                else if (request.PaymentType == "Yearly")
                {
                    request.Amount = premium.CalculatedPremium;
                }
                else
                {
                    _logger.LogWarning("Invalid PaymentType: {PaymentType}", request.PaymentType);
                    throw new Exception("Invalid PaymentType. It must be 'Monthly' or 'Yearly'.");
                }

                var calcAmount = request.Amount;
                int paymentID = 0;

                using (var connection = new SqlConnection(connectionString))
                {
                    
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("ProcessPayment", connection))
                    {
                        //command.CommandText = "ProcessPayment";
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@CustomerID", request.CustomerID));
                        command.Parameters.Add(new SqlParameter("@PolicyID", request.PolicyID));
                        command.Parameters.Add(new SqlParameter("@PremiumID", request.PremiumID));
                        command.Parameters.Add(new SqlParameter("@Amount", calcAmount));
                        command.Parameters.Add(new SqlParameter("@PaymentType", request.PaymentType));
                        command.Parameters.Add(new SqlParameter("@PaymentDate", DateTime.UtcNow));

                        var outputIdParam = new SqlParameter("@PaymentID", SqlDbType.Int) { Direction = ParameterDirection.Output };
                        command.Parameters.Add(outputIdParam);
                        await command.ExecuteNonQueryAsync();
                        paymentID = (int)outputIdParam.Value;
                    }
                }

                _logger.LogInformation("Payment processed successfully with PaymentID: {PaymentID}", paymentID);

                return new PaymentResponseDTO
                {
                    PaymentID = paymentID,
                    Amount = calcAmount,
                    PaymentDate = DateTime.UtcNow,
                    PaymentType = request.PaymentType,
                    Status = "Processed"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment for CustomerID: {CustomerID}, PolicyID: {PolicyID}", request.CustomerID, request.PolicyID);
                throw new Exception($"Error processing payment: {ex.Message}");
            }
            
        }


        public async Task<List<PaymentViewDTO>> GetPaymentsByCustomerID(int customerID)
        {
            _logger.LogInformation("Retrieving payments for CustomerID: {CustomerID}", customerID);

            try
            {
                var payments = new List<PaymentViewDTO>();
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "GetPaymentsByCustomerID";
                        command.CommandType = CommandType.StoredProcedure;

                        var param = command.CreateParameter();
                        param.ParameterName = "@CustomerID";
                        param.Value = customerID;
                        command.Parameters.Add(param);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                payments.Add(new PaymentViewDTO
                                {
                                    PaymentID = reader.GetInt32(0),
                                    CustomerID = reader.GetInt32(1),
                                    CustomerName = reader.GetString(2),
                                    PolicyID = reader.GetInt32(3),
                                    Amount = reader.GetDecimal(4),
                                    PaymentDate = reader.GetDateTime(5),
                                    PaymentType = reader.GetString(6),
                                    Status = reader.GetString(7)
                                });
                            }
                        }
                    }
                }

                _logger.LogInformation("Payments retrieved successfully for CustomerID: {CustomerID}}", customerID);
                return payments;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting payments for CustomerID: {CustomerID}", customerID);
                throw new Exception($"Error getting Payment by CustomerID: {ex.Message}");
            }

        }


        //Receipt
        public async Task<PaymentViewDTO> GenerateReceiptAsync(int paymentId)
        {
            _logger.LogInformation("Generating receipt for PaymentID: {PaymentID}", paymentId);

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("GenerateReceipt", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@PaymentID", paymentId);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var receipt = new PaymentViewDTO
                                {
                                    PaymentID = reader.GetInt32(0),
                                    CustomerID = reader.GetInt32(1),
                                    CustomerName = reader.GetString(2),
                                    PolicyID = reader.GetInt32(3),
                                    Amount = reader.GetDecimal(4),
                                    PaymentDate = reader.GetDateTime(5),
                                    PaymentType = reader.GetString(6),
                                    Status = reader.GetString(7)
                                };

                                _logger.LogInformation("Receipt generated successfully for PaymentID: {PaymentID}", paymentId);
                                return receipt;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating receipt for PaymentID: {PaymentID}", paymentId);
                throw new Exception($"Error generating receipt: {ex.Message}");
            }
            return null;
        }
    }
}
