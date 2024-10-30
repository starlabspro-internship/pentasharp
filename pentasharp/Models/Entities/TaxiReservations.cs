using System;
using System.ComponentModel.DataAnnotations;
using pentasharp.Models.Enums;
namespace pentasharp.Models.Entities
{
    public class TaxiReservations
    {
        [Key]
        public int ReservationId { get; set; }

        [Required]
        public int TaxiId { get; set; }  // Foreign key to Taxi

        [Required]
        public int UserId { get; set; }  // Foreign key to User

        [Required]
        [StringLength(256)]
        public string PickupLocation { get; set; } = string.Empty;

        [Required]
        [StringLength(256)]
        public string DropoffLocation { get; set; } = string.Empty;

        [Required]
        public DateTime ReservationTime { get; set; } = DateTime.UtcNow;

        public DateTime? TripStartTime { get; set; }

        public DateTime? TripEndTime { get; set; }

        [Required]
        public decimal Fare { get; set; }

        [Required]
        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public Taxi Taxi { get; set; } = null!; // Navigation property to Taxi
        public User User { get; set; } = null!; // Navigation property to User
    }
}
