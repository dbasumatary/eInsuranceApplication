using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_Insurance_App.Models.Entities
{
    public class InsurancePlan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PlanID { get; set; }

        [Required]
        [MaxLength(100)]
        public string PlanName { get; set; }

        [Required]
        public string PlanDetails { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Scheme> Schemes { get; set; } = new List<Scheme>();

    }
}
