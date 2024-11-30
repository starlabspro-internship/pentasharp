using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pentasharp.Models.Entities
{
    public class Notifications
    {
        /// <summary>
        /// Gets or sets the unique identifier for the notification.
        /// </summary>
        [Key]
        public int NotificationId { get; set; }

        /// <summary>
        /// Gets or sets the message content of the notification.
        /// </summary>
        [StringLength(256)]
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date and time when the notification was sent.
        /// </summary>
        [Required]
        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the foreign key for the user who will receive the notification.
        /// </summary>
        [ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the foreign key for the associated booking, if the notification is related to a taxi booking.
        /// Nullable to allow for notifications that are not associated with a booking.
        /// </summary>
        [InverseProperty("TaxiBooking")]
        public int? BookingId { get; set; }

        /// <summary>
        /// Gets or sets the user associated with this notification.
        /// </summary>
        public User User { get; set; } = null!;

        /// <summary>
        /// Gets or sets the taxi booking associated with this notification.
        /// </summary>
        public TaxiBookings TaxiBooking { get; set; } = null!;
    }
}