using E_Insurance_App.Data;
using E_Insurance_App.Models.DTOs;
using E_Insurance_App.Models.Entities;
using E_Insurance_App.Repositories.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace E_Insurance_App.Repositories.Implementation
{
    public class PremiumRepository : IPremiumRepository
    {
        private readonly InsuranceDbContext _context;
        private readonly ILogger<PremiumRepository> _logger;

        public PremiumRepository(InsuranceDbContext context, ILogger<PremiumRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<PremiumResponseDTO> CalculatePremiumAsync(PremiumCreateDTO premiumDTO)
        {
            try
            {
                decimal calculatedPremium = 0;
                int premiumID = 0;

                var connectionString = _context.Database.GetDbConnection().ConnectionString;
                if (string.IsNullOrEmpty(connectionString))
                {
                    _logger.LogError("Database connection string is null or empty!");
                    throw new Exception("Database connection string is null or empty!");
                }

                //Retrieve ustomer detail
                _logger.LogInformation("Retrieving customer details for CustomerID: {CustomerID}", premiumDTO.CustomerID);
                var customerdetail = await _context.Customers
                                   .FirstOrDefaultAsync(c => c.CustomerID == premiumDTO.CustomerID);

                if (customerdetail == null)
                {
                    _logger.LogError("Customer with ID {CustomerID} not found.", premiumDTO.CustomerID);
                    throw new Exception($"Customer with ID {premiumDTO.CustomerID} not found.");
                }

                int age = CalculateAge(customerdetail.DateOfBirth);
                _logger.LogInformation("Customer age calculated: {Age}", age);


                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("CalculatePremium", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@CustomerID", premiumDTO.CustomerID);
                        command.Parameters.AddWithValue("@PolicyID", premiumDTO.PolicyID);
                        command.Parameters.AddWithValue("@SchemeID", premiumDTO.SchemeID);
                        command.Parameters.AddWithValue("@BaseRate", premiumDTO.BaseRate);
                        command.Parameters.AddWithValue("@CustomerAge", age);

                        var outputParam = new SqlParameter("@CalculatedPremium", SqlDbType.Decimal)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outputParam);

                        _logger.LogInformation("Executing procedure to calculate premium for PolicyID: {PolicyID}", premiumDTO.PolicyID);
                        await command.ExecuteNonQueryAsync();

                        calculatedPremium = Convert.ToDecimal(outputParam.Value);
                        _logger.LogInformation("Calculated premium: {CalculatedPremium}", calculatedPremium);

                    }
                }

                // Insert into Premium Table
                var premiumEntry = new Premium
                {
                    CustomerID = premiumDTO.CustomerID,
                    PolicyID = premiumDTO.PolicyID,
                    SchemeID = premiumDTO.SchemeID,
                    BaseRate = premiumDTO.BaseRate,
                    Age = age,
                    CalculatedPremium = calculatedPremium,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Premiums.Add(premiumEntry);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Premium entry saved in db for CustomerID: {CustomerID}, PolicyID: {PolicyID}", premiumDTO.CustomerID, premiumDTO.PolicyID);


                var customer = await _context.Customers.FindAsync(premiumDTO.CustomerID);

                return new PremiumResponseDTO
                {
                    PremiumID = premiumEntry.PremiumID,
                    CustomerID = premiumEntry.CustomerID,
                    CustomerName = customer?.FullName,
                    PolicyID = premiumEntry.PolicyID,
                    SchemeID = premiumEntry.SchemeID,
                    BaseRate = premiumEntry.BaseRate,
                    Age = premiumEntry.Age,
                    CalculatedPremium = premiumEntry.CalculatedPremium,
                    CreatedAt = premiumEntry.CreatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating premium for CustomerID: {CustomerID}, PolicyID: {PolicyID}", premiumDTO.CustomerID, premiumDTO.PolicyID);
                throw new Exception($"Error calculating premium: {ex.Message}");
            }
        }

        private int CalculateAge(DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            int age = today.Year - dateOfBirth.Year;
            return age;
        }
    }
}
