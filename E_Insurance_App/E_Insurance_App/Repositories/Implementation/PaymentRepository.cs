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

        public PaymentRepository(InsuranceDbContext context)
        {
            _context = context;
            _connectionString = _context.Database.GetDbConnection().ConnectionString;
        }

        public async Task<PaymentResponseDTO> ProcessPaymentAsync(Payment request)
        {
            try
            {
                var connectionString = _context.Database.GetDbConnection().ConnectionString;
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new Exception("Database connection string is null or empty!");
                }

                var premium = await _context.Premiums.FindAsync(request.PremiumID);
                if (premium == null) throw new Exception("Invalid PremiumID");


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
                throw new Exception($"Error processing payment: {ex.Message}");
            }
            
        }


        public async Task<List<PaymentViewDTO>> GetPaymentsByCustomerID(int customerID)
        {
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
                return payments;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting Payment by CustomerID: {ex.Message}");
            }

        }
    }
}
