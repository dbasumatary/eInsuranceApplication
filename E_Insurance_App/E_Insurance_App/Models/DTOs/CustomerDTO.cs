namespace E_Insurance_App.Models.DTOs
{
    public class CustomerDTO
    {
        public int CustomerID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int AgentID { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
