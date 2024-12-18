using System;
using System.ComponentModel.DataAnnotations;

namespace pentasharp.Models.TaxiRequest
{
    /// <summary>
    /// Represents the data required to create a taxi booking.
    /// </summary>
    public class CreateBookingViewModel
    {
        /// <summary>
        /// The ID of the taxi company for the booking.
        /// </summary>
        [Required]
        public int TaxiCompanyId { get; set; }

        /// <summary>S
        /// The pickup location for the booking.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string PickupLocation { get; set; } = string.Empty;

        /// <summary>
        /// The dropoff location for the booking.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string DropoffLocation { get; set; } = string.Empty;

        /// <summary>
        /// The date and time of the booking.
        /// </summary>
        [Required]
        public DateTime BookingTime { get; set; }

        /// <summary>
        /// The number of passengers for the booking.
        /// </summary>
        [Required]
        public int PassengerCount { get; set; }

        /// <summary>
        /// The ID of the user making the booking.
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// The creation date and time of the booking.
        /// Defaults to the current date and time.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}