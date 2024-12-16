using pentasharp.Models.Enums;

namespace pentasharp.Models.TaxiRequest
{
    /// <summary>
    /// Represents the data required to edit a taxi booking.
    /// </summary>
    public class EditTaxiBookingViewModel
    {
        /// <summary>
        /// The unique identifier for the booking.
        /// </summary>
        public int BookingId { get; set; }

        /// <summary>
        /// The updated pickup location for the booking.
        /// </summary>
        public string PickupLocation { get; set; }

        /// <summary>
        /// The updated dropoff location for the booking.
        /// </summary>
        public string DropoffLocation { get; set; }

        /// <summary>
        /// The updated date and time for the booking.
        /// </summary>
        public DateTime BookingTime { get; set; }

        /// <summary>
        /// The updated status of the booking.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// The identifier of the assigned driver, if any.
        /// </summary>
        public int? DriverId { get; set; }

        /// <summary>
        /// The identifier of the assigned taxi, if any.
        /// </summary>
        public int? TaxiId { get; set; }

        /// <summary>
        /// The timestamp indicating when the booking was last updated.
        /// Defaults to the current date and time.
        /// </summary>  
        public DateTime UpdateAt { get; set; } = DateTime.Now;
    }
}