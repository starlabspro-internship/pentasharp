using pentasharp.Models.Enums;
using System;

namespace pentasharp.Models.TaxiRequest
{
    /// <summary>
    /// Represents a request to create a taxi booking.
    /// </summary>
    public class TaxiBookingRequest
    {
        /// <summary>
        /// ID of the taxi company.
        /// </summary>
        public int TaxiCompanyId { get; set; }

        /// <summary>
        /// Pickup location for the taxi booking.
        /// </summary>
        public string PickupLocation { get; set; } = string.Empty;

        /// <summary>
        /// Dropoff location for the taxi booking.
        /// </summary>
        public string DropoffLocation { get; set; } = string.Empty;

        /// <summary>
        /// Booking time for the taxi request.
        /// </summary>
        public DateTime BookingTime { get; set; }

        /// <summary>
        /// Number of passengers for the taxi booking.
        /// </summary>
        public int PassengerCount { get; set; }

        /// <summary>
        /// ID of the user making the booking.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Status of the reservation, default is <see cref="ReservationStatus.Pending"/>.
        /// </summary>
        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
    }
}