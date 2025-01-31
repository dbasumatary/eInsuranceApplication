using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_Insurance_App.Models.Entities
{
    public class Scheme
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SchemeID { get; set; }

        [Required]
        [MaxLength(100)]
        public string SchemeName { get; set; }

        [Required]
        public string SchemeDetails { get; set; }

        [Required]
        public decimal SchemeFactor { get; set; }

        [ForeignKey("InsurancePlan")]
        public int PlanID { get; set; } 

        public virtual InsurancePlan Plan { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Policy> Policies { get; set; }
        public virtual ICollection<Premium> Premiums { get; set; }
    }
}
