using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using pentasharp.Models.Enums;

namespace pentasharp.Models.Entities
{
    /// <summary>
    /// Represents a taxi booking record.
    /// </summary>
    public class TaxiBookings
    {
        /// <summary>
        /// The unique identifier for the booking.
        /// </summary>
        public int BookingId { get; set; }

        /// <summary>
        /// The identifier of the taxi company associated with the booking.
        /// </summary>
        public int TaxiCompanyId { get; set; }

        /// <summary>
        /// The pickup location for the booking.
        /// </summary>
        public string PickupLocation { get; set; } = string.Empty;

        /// <summary>
        /// The dropoff location for the booking.
        /// </summary>
        public string DropoffLocation { get; set; } = string.Empty;

        /// <summary>
        /// The time when the booking was made.
        /// </summary>
        public DateTime BookingTime { get; set; }

        /// <summary>
        /// The time when the trip started, if available.
        /// </summary>
        public DateTime? TripStartTime { get; set; }

        /// <summary>
        /// The time when the trip ended, if available.
        /// </summary>
        public DateTime? TripEndTime { get; set; }

        /// <summary>
        /// The fare for the trip, if applicable.
        /// </summary>
        public decimal? Fare { get; set; }

        /// <summary>
        /// The number of passengers for the booking.
        /// </summary>
        public int PassengerCount { get; set; }

        /// <summary>
        /// The timestamp when the booking record was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// The timestamp when the booking record was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// The identifier of the user who made the booking.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// The identifier of the taxi assigned to the booking, if applicable.
        /// </summary>
        public int? TaxiId { get; set; }

        /// <summary>
        /// The status of the reservation.
        /// </summary>
        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;

        /// <summary>
        /// The collection of notifications associated with the booking.
        /// </summary>
        public ICollection<Notifications> Notifications { get; set; } = new List<Notifications>();

        /// <summary>
        /// The taxi company associated with the booking.
        /// </summary>
        public TaxiCompany TaxiCompany { get; set; } = null!;

        /// <summary>
        /// The user who made the booking.
        /// </summary>
        public User User { get; set; } = null!;

        /// <summary>
        /// The taxi assigned to the booking.
        /// </summary>
        public Taxi Taxi { get; set; } = null!;
    }
}