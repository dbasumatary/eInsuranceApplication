using System.ComponentModel.DataAnnotations;

namespace E_Insurance_App.Models.DTOs
{
    public class CommissionDTO
    {
        [Required]
        public int AgentID { get; set; }

    }
}
