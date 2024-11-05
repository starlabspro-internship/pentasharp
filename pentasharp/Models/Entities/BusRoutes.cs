using System.ComponentModel.DataAnnotations;

namespace pentasharp.Models.Entities
{
    public class BusRoutes
    {
        [Key]
        public int RouteId { get; set; }  

        [Required]
        [MaxLength(100)]
        public string FromLocation { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string ToLocation { get; set; } = string.Empty;

        [Required]
        public TimeSpan EstimatedDuration { get; set; } 

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }  
    }
}
