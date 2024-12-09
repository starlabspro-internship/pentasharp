namespace pentasharp.Models.TaxiRequest
{
    /// <summary>
    /// Represents the detailed view model for a taxi booking.
    /// </summary>
    public class TaxiBookingViewModel
    {
        /// <summary>
        /// Unique identifier of the booking.
        /// </summary>
        public int BookingId { get; set; }

        /// <summary>
        /// Name of the passenger associated with the booking.
        /// </summary>
        public string PassengerName { get; set; }

        /// <summary>
        /// Location where the passenger will be picked up.
        /// </summary>
        public string PickupLocation { get; set; }

        /// <summary>
        /// Location where the passenger will be dropped off.
        /// </summary>
        public string DropoffLocation { get; set; }

        /// <summary>
        /// Booking time in string format.
        /// </summary>
        public string BookingTime { get; set; }

        /// <summary>
        /// Current status of the booking.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Name of the assigned driver, or "No Driver Assign" if no driver is assigned.
        /// </summary>
        public string DriverName { get; set; } = "No Driver Assign";

        /// <summary>
        /// Identifier of the assigned driver, if any.
        /// </summary>
        public int? DriverId { get; set; }

        /// <summary>
        /// Identifier of the assigned taxi, if any.
        /// </summary>
        public int? TaxiId { get; set; }
    }
}