using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_Insurance_App.Models.Entities
{
    public class Commission
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommissionID { get; set; }

        [Required]
        public int AgentID { get; set; }

        [Required]
        [MaxLength(100)]
        public string AgentName { get; set; }

        [Required]
        public int PolicyID { get; set; }

        [Required]
        public int PremiumID { get; set; }

        [Required]
        public decimal CommissionAmount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsPaid { get; set; } = false;
        public DateTime? PaymentProcessedDate { get; set; }

        public virtual Agent Agent { get; set; }
        public virtual Policy Policy { get; set; }
        public virtual Premium Premium { get; set; }
    }
}
