using System.ComponentModel.DataAnnotations;

namespace E_Insurance_App.Models.DTOs
{
    public class PolicyCreateDTO
    {
        [Required]
        public int CustomerID { get; set; }

        [Required]
        public int SchemeID { get; set; }

        [Required]
        public string PolicyDetails { get; set; }

        [Required]
        public DateTime DateIssued { get; set; }

        [Required]
        public int MaturityPeriod { get; set; }

        [Required]
        public DateTime PolicyLapseDate { get; set; }
    }
}
