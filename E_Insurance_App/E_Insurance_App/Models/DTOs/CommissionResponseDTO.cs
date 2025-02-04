namespace E_Insurance_App.Models.DTOs
{
    public class CommissionResponseDTO
    {
        public int CommissionID { get; set; }
        public int AgentID { get; set; }
        public string AgentName { get; set; }
        public int PolicyID { get; set; }
        public int PremiumID { get; set; }
        public decimal CommissionAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsPaid { get; set; } 
        public DateTime? PaymentProcessedDate { get; set; }
    }
}
