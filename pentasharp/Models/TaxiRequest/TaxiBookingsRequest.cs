using System;
using System.Collections.Generic;
using pentasharp.Models.Entities;
using pentasharp.Models.Enums;

namespace pentasharp.Models.TaxiRequests
{
    /// <summary>
    /// Represents a taxi booking request including details of the booking, user, and associated entities.
    /// </summary>
    public class TaxiBookingRequest
    {
        /// <summary>
        /// The unique identifier for the booking.
        /// </summary>
        public int BookingId { get; set; }

        /// <summary>
        /// The ID of the taxi company for this booking.
        /// </summary>
        public int TaxiCompanyId { get; set; }

        /// <summary>
        /// The location where the passenger will be picked up.
        /// </summary>
        public string PickupLocation { get; set; } = string.Empty;

        /// <summary>
        /// The location where the passenger will be dropped off.
        /// </summary>
        public string DropoffLocation { get; set; } = string.Empty;

        /// <summary>
        /// The date and time of the booking.
        /// </summary>
        public DateTime BookingTime { get; set; }

        /// <summary>
        /// The date and time when the trip started, if applicable.
        /// </summary>
        public DateTime? TripStartTime { get; set; }

        /// <summary>
        /// The date and time when the trip ended, if applicable.
        /// </summary>
        public DateTime? TripEndTime { get; set; }

        /// <summary>
        /// The fare for the trip, if calculated.
        /// </summary>
        public decimal? Fare { get; set; }

        /// <summary>
        /// The number of passengers for this booking.
        /// </summary>
        public int PassangerCount { get; set; }

        /// <summary>
        /// The date and time when the booking was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// The date and time when the booking was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// The ID of the user associated with the booking.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// The ID of the taxi assigned to the booking, if any.
        /// </summary>
        public int? TaxiId { get; set; }

        /// <summary>
        /// The current status of the booking.
        /// </summary>
        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;

        /// <summary>
        /// The list of notifications associated with this booking.
        /// </summary>
        public ICollection<Notifications> Notifications { get; set; } = new List<Notifications>();

        /// <summary>
        /// The taxi company associated with this booking.
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