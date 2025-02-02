namespace E_Insurance_App.Models.DTOs
{
    public class PaymentDTO
    {
        public int CustomerID { get; set; }
        public int PolicyID { get; set; }
        public int PremiumID { get; set; }
        public string PaymentType { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
