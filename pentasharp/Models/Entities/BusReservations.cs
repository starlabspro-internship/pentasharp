using System;
using System.ComponentModel.DataAnnotations;
using pentasharp.Models.Enums;

namespace pentasharp.Models.Entities
{
    public class BusReservations
    {
        [Key]
        public int ReservationId { get; set; } 

        [Required]
        public DateTime ReservationDate { get; set; }  

        [Required]
        public int NumberOfSeats { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }  

        [Required]
        public PaymentStatus PaymentStatus { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        [Required]
        public int ScheduleId { get; set; } 

        [Required]
        public int UserId { get; set; } 

        [Required]
        public BusReservationStatus Status { get; set; } = BusReservationStatus.Pending;  

        public BusSchedule Schedule { get; set; } = null!;  
        public User User { get; set; } = null!;  
    }
}

