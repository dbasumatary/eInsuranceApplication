using System.ComponentModel.DataAnnotations;

namespace E_Insurance_App.Models.DTOs
{
    public class PolicyPurchaseDTO
    {
        [Required]
        public int CustomerID { get; set; }

        [Required]
        public int SchemeID { get; set; }

        [Required]
        public string PolicyDetails { get; set; }

        [Required]
        public decimal BaseRate { get; set; }

        [Required]
        public int MaturityPeriod { get; set; }
    }
}
