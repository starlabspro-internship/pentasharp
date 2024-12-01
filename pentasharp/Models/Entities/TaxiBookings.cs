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
        public int TaxiCompanyId { get; set; }

        [Required]
        [StringLength(100)]
        public string PickupLocation { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string DropoffLocation { get; set; } = string.Empty;

        public DateTime BookingTime { get; set; }

        public DateTime? TripStartTime { get; set; } 

        public DateTime? TripEndTime { get; set; } 

        public decimal? Fare { get; set; }

        [Required]
        public int PassangerCount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [ForeignKey("Taxi")]
        public int? TaxiId { get; set; }

        [Required]
        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;

        public ICollection<Notifications> Notifications { get; set; } = new List<Notifications>();

        public TaxiCompany TaxiCompany { get; set; } = null!;
        public User User { get; set; } = null!;
        public Taxi Taxi { get; set; } = null!;
    }
}