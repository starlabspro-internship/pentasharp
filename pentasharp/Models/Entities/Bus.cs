using pentasharp.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pentasharp.Models.Entities
{
    public class Bus
    {
        [Key]
        public int BusId { get; set; }

        [Required]
        [ForeignKey("BusCompany")]
        public int BusCompanyId { get; set; }

        [Required]
        public int Capacity { get; set; }
        public BusStatus Status { get; set; } = BusStatus.Available;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public BusCompany BusCompany { get; set; } = null!;

    }
}
