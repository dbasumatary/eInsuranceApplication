namespace E_Insurance_App.Models.DTOs
{
    public class PaymentViewDTO
    {
        public int PaymentID { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public int PolicyID { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentType { get; set; }
        public string Status { get; set; }
    }
}
