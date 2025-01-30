using E_Insurance_App.Models.DTOs;
using E_Insurance_App.Models.Entities;

namespace E_Insurance_App.Services.Interface
{
    public interface ISchemeService
    {
        Task<Scheme> RegisterSchemeAsync(SchemeDTO schemeDto);
    }
}
