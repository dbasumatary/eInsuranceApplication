using System.ComponentModel.DataAnnotations;

namespace E_Insurance_App.Models.DTOs
{
    public class AgentRegisterDTO
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public decimal CommissionRate { get; set; }
    }
}
