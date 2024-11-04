using System;
using System.ComponentModel.DataAnnotations;
using pentasharp.Models.Enums;

namespace pentasharp.Models.Entities
{
    public class BusSchedule
    {
        [Key]
        public int ScheduleId { get; set; }  

        [Required]
        public DateTime DepartureTime { get; set; }  

        [Required]
        public DateTime ArrivalTime { get; set; }  

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int AvailableSeats { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;  

        public DateTime? UpdatedAt { get; set; }  

        [Required]
        public int BusId { get; set; }  

        [Required]
        public int RouteId { get; set; }  

        [Required]
        public BusScheduleStatus Status { get; set; } = BusScheduleStatus.Scheduled;  

        public Buses Bus { get; set; } = null!;  
        public BusRoutes Route { get; set; } = null!;  
    }
}
