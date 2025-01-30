using E_Insurance_App.Models.Entities;

namespace E_Insurance_App.Repositories.Interface
{
    public interface ISchemeRepository
    {
        Task<Scheme> AddSchemeAsync(Scheme scheme);
    }
}
