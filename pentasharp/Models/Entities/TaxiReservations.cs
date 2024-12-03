using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using pentasharp.Models.Enums;

namespace pentasharp.Models.Entities
{
    /// <summary>
    /// Represents a reservation for a taxi.
    /// </summary>
    public class TaxiReservations
    {
        /// <summary>
        /// Gets or sets the unique identifier for the reservation.
        /// </summary>
        [Key]
        public int ReservationId { get; set; }

        /// <summary>
        /// Gets or sets the taxi ID associated with the reservation. This is optional initially.
        /// </summary>
        [Required]
        public int? TaxiId { get; set; } = null;

        /// <summary>
        /// Gets or sets the taxi company ID associated with the reservation.
        /// </summary>
        [Required]
        [ForeignKey("TaxiCompany")]
        public int TaxiCompanyId { get; set; }

        /// <summary>
        /// Gets or sets the user ID who made the reservation.
        /// </summary>
        [Required]
        public int UserId { get; set; } // Foreign key to User

        /// <summary>
        /// Gets or sets the pickup location for the trip.
        /// </summary>
        [Required]
        [StringLength(256)]
        public string PickupLocation { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the drop-off location for the trip.
        /// </summary>
        [Required]
        [StringLength(256)]
        public string DropoffLocation { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the time when the reservation was made.
        /// </summary>
        [Required]
        public DateTime ReservationTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the start time of the trip, if available.
        /// </summary>
        public DateTime? TripStartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time of the trip, if available.
        /// </summary>
        public DateTime? TripEndTime { get; set; }

        /// <summary>
        /// Gets or sets the fare amount for the trip.
        /// </summary>
        [Required]
        public decimal? Fare { get; set; }

        /// <summary>
        /// Gets or sets the status of the reservation.
        /// </summary>
        [Required]
        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;

        /// <summary>
        /// Gets or sets the creation date and time of the reservation.
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the number of passengers for the trip.
        /// </summary>
        [Required]
        public int PassengerCount { get; set; }

        /// <summary>
        /// Gets or sets the last updated date and time of the reservation.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Navigation property to the associated Taxi entity.
        /// </summary>
        public Taxi Taxi { get; set; } = null!;

        /// <summary>
        /// Navigation property to the associated TaxiCompany entity.
        /// </summary>
        public TaxiCompany TaxiCompany { get; set; } = null!;

        /// <summary>
        /// Navigation property to the associated User entity.
        /// </summary>
        public User User { get; set; } = null!;
    }
}