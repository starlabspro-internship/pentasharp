using System;
using System.ComponentModel.DataAnnotations;
using pentasharp.Models.Enums;

namespace pentasharp.ViewModel.BusSchedul
{
    /// <summary>
    /// Represents the data required to add a new bus schedule.
    /// </summary>
    public class AddScheduleViewModel
    {
        /// <summary>
        /// Unique identifier for the schedule.
        /// </summary>
        public int ScheduleId { get; set; }

        /// <summary> 
        /// Departure time of the bus schedule.
        /// </summary>
        [Required]
        public DateTime DepartureTime { get; set; }

        /// <summary>
        /// Arrival time of the bus schedule.
        /// </summary>
        public DateTime ArrivalTime { get; set; }

        /// <summary>
        /// Price for the bus schedule.
        /// </summary>
        [Required]
        public decimal Price { get; set; }

        /// <summary>
        /// Number of available seats for the bus schedule.
        /// </summary>
        [Required]
        public int AvailableSeats { get; set; }

        /// <summary>
        /// Unique identifier for the bus assigned to the schedule.
        /// </summary>
        [Required]
        public int BusId { get; set; }

        /// <summary>
        /// Unique identifier for the route associated with the schedule.
        /// </summary>
        public int RouteId { get; set; }

        /// <summary>
        /// Unique identifier for the bus company that owns this schedule.
        /// </summary>
        public int BusCompanyId { get; set; }

        /// <summary>
        /// Status of the bus schedule (Scheduled, Canceled, etc.).
        /// </summary>
        public BusScheduleStatus Status { get; set; } = BusScheduleStatus.Scheduled;
    }
}