using E_Insurance_App.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace E_Insurance_App.Models.DTOs
{
    public class PremiumResponseDTO
    {
        public int PremiumID { get; set; }

        public int CustomerID { get; set; }
        public string CustomerName { get; set; }

        public int PolicyID { get; set; }

        public int SchemeID { get; set; }

        public decimal BaseRate { get; set; }

        public int Age { get; set; }

        public decimal CalculatedPremium { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }

}
