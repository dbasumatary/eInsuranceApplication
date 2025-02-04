using System.ComponentModel.DataAnnotations;

namespace E_Insurance_App.Models.DTOs
{
    public class PolicyCancellationDTO
    {
        public int PolicyID { get; set; }
        public string Reason { get; set; }
        public string CancelledBy { get; set; }
    }
}
