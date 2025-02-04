using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_Insurance_App.Models.Entities
{
    public class PolicyCancellation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CancellationID { get; set; }

        [Required]
        [ForeignKey("PolicyID")]
        public int PolicyID { get; set; }
        
        public virtual Policy Policy { get; set; }

        [Required]
        public string Reason { get; set; }

        [Required]
        public DateTime CancellationDate { get; set; } = DateTime.UtcNow;

        [Required]
        public string CancelledBy { get; set; }
    }
}
