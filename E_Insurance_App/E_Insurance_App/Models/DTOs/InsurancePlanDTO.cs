using System.ComponentModel.DataAnnotations;

namespace E_Insurance_App.Models.DTOs
{
    public class InsurancePlanDTO
    {
        [Required]
        [MaxLength(100)]
        public string PlanName { get; set; }

        [Required]
        public string PlanDetails { get; set; }
    }
}
