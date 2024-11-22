using System;
using System.ComponentModel.DataAnnotations;
using pentasharp.Models.Enums;

namespace pentasharp.ViewModel.BusSchedul
{
    /// <summary>
    /// Represents the data required to edit an existing bus schedule.
    /// </summary>
    public class EditScheduleViewModel
    {
        /// <summary>
        /// Unique identifier for the route associated with the schedule.
        /// </summary>
        [Required]
        public int RouteId { get; set; }

        /// <summary>
        /// Unique identifier for the bus assigned to the schedule.
        /// </summary>
        [Required]
        public int BusId { get; set; }

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
    }
}