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

        public PremiumRepository(InsuranceDbContext context)
        {
            _context = context;
        }

        public async Task<PremiumResponseDTO> CalculatePremiumAsync(PremiumCreateDTO premiumDTO)
        {
            try
            {
                int calculatedPremium = 0;
                int premiumID = 0;

                var connectionString = _context.Database.GetDbConnection().ConnectionString;
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new Exception("Database connection string is null or empty!");
                }

                //Retrieve ustomer detail
                var customerdetail = await _context.Customers
                                   .FirstOrDefaultAsync(c => c.CustomerID == premiumDTO.CustomerID);

                if (customerdetail == null)
                {
                    throw new Exception($"Customer with ID {premiumDTO.CustomerID} not found.");
                }

                int age = CalculateAge(customerdetail.DateOfBirth);

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

                        await command.ExecuteNonQueryAsync();

                        calculatedPremium = Convert.ToInt32(outputParam.Value);
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
