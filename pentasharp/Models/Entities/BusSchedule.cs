using System;
using System.ComponentModel.DataAnnotations;
using pentasharp.Models.Enums;

namespace pentasharp.Models.Entities
{
    /// <summary>
    /// Represents the scheduling details for a bus, including timing, pricing, and capacity.
    /// </summary>
    public class BusSchedule
    {
        /// <summary>
        /// Gets or sets the unique identifier for the bus schedule.
        /// </summary>
        [Key]
        public int ScheduleId { get; set; }

        /// <summary>
        /// Gets or sets the departure time of the bus.
        /// </summary>
        [Required]
        public DateTime DepartureTime { get; set; }

        /// <summary>
        /// Gets or sets the arrival time of the bus.
        /// </summary>
        [Required]
        public DateTime ArrivalTime { get; set; }

        /// <summary>
        /// Gets or sets the ticket price for a single seat on this bus schedule.
        /// </summary>
        [Required]
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the number of available seats on this bus.
        /// </summary>
        [Required]
        public int AvailableSeats { get; set; }

        /// <summary>
        /// Gets or sets the creation date and time of the schedule, defaulting to the current UTC time.
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the last updated date and time of the schedule. Null if never updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the ID of the bus associated with this schedule.
        /// </summary>
        [Required]
        public int BusId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the route that this schedule is for.
        /// </summary>
        [Required]
        public int RouteId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the associated bus company.
        /// </summary>
        public int BusCompanyId { get; set; }

        /// <summary>
        /// Gets or sets the bus company associated with this bus.
        /// </summary>
        public BusCompany BusCompany { get; set; } = null!;

        /// <summary>
        /// Gets or sets the status of the bus schedule.
        /// </summary>
        [Required]
        public BusScheduleStatus Status { get; set; } = BusScheduleStatus.Scheduled;

        /// <summary>
        /// Navigation property for accessing the bus associated with this schedule.
        /// </summary>
        public Buses Bus { get; set; } = null!;

        /// <summary>
        /// Navigation property for accessing the route associated with this schedule.
        /// </summary>
        public BusRoutes Route { get; set; } = null!;
    }
}
