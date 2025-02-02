using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_Insurance_App.Models.Entities
{
    public class Payment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentID { get; set; }

        [Required]
        public int CustomerID { get; set; }

        [Required]
        public int PolicyID { get; set; }

        [Required]
        public int PremiumID { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        [Required]
        [MaxLength(50)]
        public string PaymentType { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Pending";

        public virtual Customer Customer { get; set; }
        public virtual Policy Policy { get; set; }
        public virtual Premium Premium { get; set; }
    }
}
