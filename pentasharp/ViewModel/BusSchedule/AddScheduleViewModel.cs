using System;
using System.ComponentModel.DataAnnotations;
using pentasharp.Models.Enums;

namespace pentasharp.ViewModel.BusSchedule
{
    public class AddScheduleViewModel
    {
        public int ScheduleId { get; set; }

        [Required]
        public DateTime DepartureTime { get; set; }

        public DateTime ArrivalTime { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int AvailableSeats { get; set; }

        [Required]
        public int BusId { get; set; }

        public int RouteId { get; set; }

        public BusScheduleStatus Status { get; set; } = BusScheduleStatus.Scheduled;
    }
}