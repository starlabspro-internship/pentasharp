using System;
using System.ComponentModel.DataAnnotations;

namespace pentasharp.Models.TaxiRequest
{
    /// <summary>
    /// Represents the view model for creating a new taxi reservation.
    /// </summary>
    public class TaxiReservationViewModel
    {
        /// <summary>
        /// Gets or sets the identifier of the taxi company for the reservation.
        /// This is a required field.
        /// </summary>
        [Required]
        public int TaxiCompanyId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user making the reservation.
        /// This is a required field.
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the pickup location for the reservation.
        /// This is a required field with a maximum length of 256 characters.
        /// </summary>
        [Required]
        [StringLength(256)]
        public string PickupLocation { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the dropoff location for the reservation.
        /// This is a required field with a maximum length of 256 characters.
        /// </summary>
        [Required]
        [StringLength(256)]
        public string DropoffLocation { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the reservation date.
        /// This is a required field.
        /// </summary>
        [Required]
        public DateTime ReservationDate { get; set; }

        /// <summary>
        /// Gets or sets the reservation time as a string.
        /// This is a required field.
        /// </summary>
        [Required]
        public string ReservationTime { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the number of passengers for the reservation.
        /// This is a required field.
        /// </summary>
        [Required]
        public int PassengerCount { get; set; }
    }
}