using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pentasharp.Models.Entities
{
    public class Notifications
    {
        [Key]
        public int NotificationId { get; set; }

        [Required]
        [StringLength(256)]
        public string Message { get; set; } = string.Empty;

        [Required]
        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        [InverseProperty("TaxiBooking")]
        public int? BookingId { get; set; }

        public User User { get; set; } = null!;
        public TaxiBookings TaxiBooking { get; set; } = null!;


    }
}