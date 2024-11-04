using System;
using System.ComponentModel.DataAnnotations;

namespace pentasharp.Models.Entities
{
    public class BusRouteAssignments
    {
        [Key]
        public int AssignmentId { get; set; } 

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;  

        public DateTime? UpdatedAt { get; set; }  

        [Required]
        public int BusId { get; set; }  

        [Required]
        public int RouteId { get; set; }  

        public Buses Bus { get; set; } = null!;  
        public BusRoutes Route { get; set; } = null!;  
    }
}
