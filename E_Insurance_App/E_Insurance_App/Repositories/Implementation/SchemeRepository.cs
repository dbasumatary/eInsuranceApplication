using E_Insurance_App.Data;
using E_Insurance_App.Models.Entities;
using E_Insurance_App.Repositories.Interface;

namespace E_Insurance_App.Repositories.Implementation
{
    public class SchemeRepository : ISchemeRepository
    {
        private readonly InsuranceDbContext _context;

        public SchemeRepository(InsuranceDbContext context)
        {
            _context = context;
        }

        public async Task<Scheme> AddSchemeAsync(Scheme scheme)
        {
            try
            {
                _context.Schemes.Add(scheme);
                await _context.SaveChangesAsync();
                return scheme;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding schemes: {ex.Message}");
            }
        }
    }
}
