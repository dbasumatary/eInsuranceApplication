using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_Insurance_App.Models.Entities
{
    public class Premium
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PremiumID { get; set; }

        [Required]
        public int CustomerID { get; set; }

        [Required]
        public int PolicyID { get; set; }

        [Required]
        public int SchemeID { get; set; }

        [Required]
        public decimal BaseRate { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        public decimal CalculatedPremium { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual Customer Customer { get; set; }
        public virtual Policy Policy { get; set; }
        public virtual Scheme Scheme { get; set; }
    }
}
