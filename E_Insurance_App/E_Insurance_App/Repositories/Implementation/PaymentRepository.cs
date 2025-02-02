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

        public PaymentRepository(InsuranceDbContext context)
        {
            _context = context;
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
    }
}
