namespace E_Insurance_App.Models.DTOs
{
    public class PremiumCreateDTO
    {
        public int CustomerID { get; set; }
        public int PolicyID { get; set; }
        public int SchemeID { get; set; }
        public decimal BaseRate { get; set; }
    }
}
