using System.ComponentModel.DataAnnotations;

namespace pentasharp.Models.TaxiRequest
{
    /// <summary>
    /// Represents a DTO for a taxi reservation, including details about the reservation, user, and taxi.
    /// </summary>
    public class TaxiReservationRequest
    {
        /// <summary>
        /// Gets or sets the unique identifier for the taxi company associated with the reservation.
        /// </summary>
        public int TaxiCompanyId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the user who made the reservation.
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// Gets or sets the pickup location for the reservation.
        /// </summary>
        public string PickupLocation { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the dropoff location for the reservation.
        /// </summary>
        public string DropoffLocation { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the formatted reservation time as a string.
        /// </summary>
        public string ReservationTime { get; set; }

        /// <summary>
        /// Gets or sets the number of passengers for the reservation.
        /// </summary>
        public int PassengerCount { get; set; }

        /// <summary>
        /// Gets or sets the status of the reservation (e.g., confirmed, pending).
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the reservation was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the reservation.
        /// </summary>
        public int ReservationId { get; set; }

        /// <summary>
        /// Gets or sets the name of the passenger who made the reservation.
        /// </summary>
        public string PassengerName { get; set; }

        /// <summary>
        /// Gets or sets the formatted reservation date as a string.
        /// </summary>
        public string ReservationDate { get; set; }

        /// <summary>
        /// Gets or sets the details of the driver and taxi (e.g., driver name and license plate).
        /// </summary>
        public string Driver { get; set; }

        /// <summary>
        /// The name of the driver.
        /// </summary>
        public string DriverName { get; set; }

        /// <summary>
        /// The unique identifier of the taxi associated with the driver.
        /// </summary>
        public int TaxiId { get; set; }

        /// <summary>
        /// Gets or sets the fare.
        /// </summary>
        public decimal? Fare { get; set; }
    }
}