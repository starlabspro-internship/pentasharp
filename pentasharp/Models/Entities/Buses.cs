using pentasharp.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pentasharp.Models.Entities
{
    public class Buses
    {
        [Key]
        public int BusId { get; set; }

        [Required]
        [ForeignKey("BusCompany")]
        public int BusCompanyId { get; set; }
        public BusStatus Status { get; set; } = BusStatus.Active;

        [Required]
        public int BusNumber { get; set; }

        [Required]
        public int Capacity { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public BusCompany BusCompany { get; set; } = null!;

    }
}
