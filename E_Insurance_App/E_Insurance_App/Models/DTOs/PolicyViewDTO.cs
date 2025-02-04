namespace E_Insurance_App.Models.DTOs
{
    public class PolicyViewDTO
    {
        public int PolicyID { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public int SchemeID { get; set; }
        public string PolicyDetails { get; set; }
        public decimal Premium { get; set; }
        public DateTime DateIssued { get; set; }
        public int MaturityPeriod { get; set; }
        public DateTime PolicyLapseDate { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
