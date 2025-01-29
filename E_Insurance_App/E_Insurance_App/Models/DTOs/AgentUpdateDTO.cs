using System.ComponentModel.DataAnnotations;

namespace E_Insurance_App.Models.DTOs
{
    public class AgentUpdateDTO
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public decimal CommissionRate { get; set; }
    }
}
