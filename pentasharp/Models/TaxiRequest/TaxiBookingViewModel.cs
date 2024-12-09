namespace pentasharp.Models.TaxiRequest
{
    /// <summary>
    /// Represents the detailed view model for a taxi booking.
    /// </summary>
    public class TaxiBookingViewModel
    {
        /// <summary>
        /// The unique identifier of the booking.
        /// </summary>
        public int BookingId { get; set; }

        /// <summary>
        /// The name of the passenger associated with the booking.
        /// </summary>
        public string PassengerName { get; set; }

        /// <summary>
        /// The location where the passenger will be picked up.
        /// </summary>
        public string PickupLocation { get; set; }

        /// <summary>
        /// The location where the passenger will be dropped off.
        /// </summary>
        public string DropoffLocation { get; set; }

        /// <summary>
        /// The booking time in string format.
        /// </summary>
        public string BookingTime { get; set; }

        /// <summary>
        /// The current status of the booking.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// The name of the assigned driver, or "No Driver Assign" if no driver is assigned.
        /// </summary>
        public string DriverName { get; set; } = "No Driver Assign";

        public int? DriverId { get; set; }

        /// <summary>
        /// The identifier of the assigned taxi, if any.
        /// </summary>
        public int? TaxiId { get; set; }
    }
}