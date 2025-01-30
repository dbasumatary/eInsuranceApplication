using System.ComponentModel.DataAnnotations;

namespace E_Insurance_App.Models.DTOs
{
    public class SchemeDTO
    {
        [Required]
        [MaxLength(100)]
        public string SchemeName { get; set; }

        [Required]
        public string SchemeDetails { get; set; }

        [Required]
        public decimal SchemeFactor { get; set; }

        [Required]
        public int PlanID { get; set; }

    }
}
