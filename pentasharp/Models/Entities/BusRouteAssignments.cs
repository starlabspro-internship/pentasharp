using System;
using System.ComponentModel.DataAnnotations;

namespace pentasharp.Models.Entities
{
    /// <summary>
    /// Represents an assignment of a bus to a specific route.
    /// </summary>
    public class BusRouteAssignments
    {
        /// <summary>
        /// Gets or sets the primary key for the bus route assignment.
        /// </summary>
        [Key]
        public int AssignmentId { get; set; }

        /// <summary>
        /// Gets or sets the creation date and time of the assignment, defaulting to the current UTC time.
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the last updated date and time of the assignment. Null if never updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the ID of the bus involved in the assignment.
        /// </summary>
        [Required]
        public int BusId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the route to which the bus is assigned.
        /// </summary>
        [Required]
        public int RouteId { get; set; }

        /// <summary>
        /// Navigation property for accessing the bus involved in the assignment.
        /// </summary>
        public Buses Bus { get; set; } = null!;

        /// <summary>
        /// Navigation property for accessing the route associated with the assignment.
        /// </summary>
        public BusRoutes Route { get; set; } = null!;
    }
}
