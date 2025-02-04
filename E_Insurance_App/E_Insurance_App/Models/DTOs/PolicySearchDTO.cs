namespace E_Insurance_App.Models.DTOs
{
    public class PolicySearchDTO
    {
        public int? PolicyId { get; set; }
        public string? CustomerName { get; set; }
        public int? AgentID { get; set; }
        public int? EmployeeID { get; set; }
    }
}
