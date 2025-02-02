namespace E_Insurance_App.Models.DTOs
{
    public class PaymentResponseDTO
    {
        public int PaymentID { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentType { get; set; }
        public string Status { get; set; }
    }
}
