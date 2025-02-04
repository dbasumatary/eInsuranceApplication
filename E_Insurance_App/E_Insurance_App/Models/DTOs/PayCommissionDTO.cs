namespace E_Insurance_App.Models.DTOs
{
    public class PayCommissionDTO
    {
        public int AgentID { get; set; }
        public List<int> CommissionIDs { get; set; }
    }
}
