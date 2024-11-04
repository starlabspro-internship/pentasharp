using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using pentasharp.Models.Enums;

namespace pentasharp.Models.Entities
{
    public class TaxiBookings
    {
        [Key]
        public int BookingId { get; set; }

        [Required]
        [StringLength(100)]
        public string PickupLocation { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string DropoffLocation { get; set; } = string.Empty;

        [Required]
        public DateTime BookingTime { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime TripStartTime { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime TripEndTime { get; set; } = DateTime.UtcNow;

        [Required]
        public string Fare { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        [ForeignKey("Taxi")]
        public int TaxiId { get; set; }

        [Required]
        public BookingStatus Status { get; set; } = BookingStatus.Pending;

        public ICollection<Notifications> Notifications { get; set; } = new List<Notifications>();


        public User User { get; set; } = null!;
        public Taxi Taxi { get; set; } = null!;


    }
}