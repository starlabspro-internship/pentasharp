using System.ComponentModel.DataAnnotations;

namespace pentasharp.Models.TaxiRequest
{
    /// <summary>
    /// Represents the view model used to update an existing taxi reservation.
    /// </summary>
    public class UpdateTaxiReservationViewModel
    {
        /// <summary>
        /// Gets or sets the identifier of the taxi assigned to the reservation.
        /// If no taxi is assigned, this can be null.
        /// </summary>
        public int? TaxiId { get; set; }

        /// <summary>
        /// Gets or sets the name of the driver for the taxi.
        /// This can be null if no driver is assigned.
        /// </summary>
        public string? DriverName { get; set; }

        /// <summary>
        /// Gets or sets the fare for the reservation.
        /// This can be null if the fare is not specified.
        /// </summary>
        public decimal? Fare { get; set; }

        /// <summary>
        /// Gets or sets the pickup location for the reservation.
        /// This can be null if no location is provided.
        /// </summary>
        public string? PickupLocation { get; set; }

        /// <summary>
        /// Gets or sets the dropoff location for the reservation.
        /// This can be null if no location is provided.
        /// </summary>
        public string? DropoffLocation { get; set; }

        /// <summary>
        /// Gets or sets the date of the reservation.
        /// This can be null if the reservation date is not specified.
        /// </summary>
        public DateTime? ReservationDate { get; set; }

        /// <summary>
        /// Gets or sets the time of the reservation in string format.
        /// This can be null if the reservation time is not specified.
        /// </summary>
        public string? ReservationTime { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the status of the reservation.
        /// This can be null if the status is not specified.
        /// </summary>
        public string? Status { get; set; }
    }
}