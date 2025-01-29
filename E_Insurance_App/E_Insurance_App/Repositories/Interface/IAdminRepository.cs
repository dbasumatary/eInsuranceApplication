using E_Insurance_App.Models.Entities;

namespace E_Insurance_App.Repositories.Interface
{
    public interface IAdminRepository
    {
        Task<Admin> RegisterAdminAsync(Admin admin);
    }
}
