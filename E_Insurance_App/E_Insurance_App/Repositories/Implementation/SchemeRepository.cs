using E_Insurance_App.Data;
using E_Insurance_App.Models.Entities;
using E_Insurance_App.Repositories.Interface;

namespace E_Insurance_App.Repositories.Implementation
{
    public class SchemeRepository : ISchemeRepository
    {
        private readonly InsuranceDbContext _context;
        private readonly ILogger<SchemeRepository> _logger;

        public SchemeRepository(InsuranceDbContext context, ILogger<SchemeRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Scheme> AddSchemeAsync(Scheme scheme)
        {
            _logger.LogInformation("Adding new scheme: {SchemeName}", scheme.SchemeName);

            try
            {
                _context.Schemes.Add(scheme);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Scheme added successfully with ID: {SchemeID}", scheme.SchemeID);
                return scheme;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during adding scheme: {SchemeName}", scheme.SchemeName);
                throw new Exception($"Error adding schemes: {ex.Message}");
            }
        }
    }
}
