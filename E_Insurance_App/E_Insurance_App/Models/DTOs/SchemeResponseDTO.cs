namespace E_Insurance_App.Models.DTOs
{
    public class SchemeResponseDTO
    {
        public int SchemeID { get; set; }
        public string SchemeName { get; set; }
        public string SchemeDetails { get; set; }
        public decimal SchemeFactor { get; set; }
        public int PlanID { get; set; }
    }
}
