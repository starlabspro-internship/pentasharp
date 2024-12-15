using pentasharp.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace pentasharp.Models.BusRequests
{
    /// <summary>
    /// Represents the data required to add a new bus schedule.
    /// </summary>
    public class AddScheduleRequest
    {
        /// <summary>
        /// Unique identifier for the schedule.
        /// </summary>
        public int ScheduleId { get; set; }

        /// <summary> 
        /// Departure time of the bus schedule.
        /// </summary>
        public DateTime DepartureTime { get; set; }

        /// <summary>
        /// Arrival time of the bus schedule.
        /// </summary>
        public DateTime ArrivalTime { get; set; }

        /// <summary>
        /// Price for the bus schedule.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Number of available seats for the bus schedule.
        /// </summary>
        public int AvailableSeats { get; set; }

        /// <summary>
        /// Unique identifier for the bus assigned to the schedule.
        /// </summary>
        public int BusId { get; set; }

        /// <summary>
        /// Unique identifier for the route associated with the schedule.
        /// </summary>
        public int RouteId { get; set; }

        public int BusCompanyId { get; set; }

        /// <summary>
        /// Status of the bus schedule (Scheduled, Canceled, etc.).
        /// </summary>
        public BusScheduleStatus Status { get; set; } = BusScheduleStatus.Scheduled;
    }
}