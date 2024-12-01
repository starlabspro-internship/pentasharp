using System;
using System.ComponentModel.DataAnnotations;

namespace pentasharp.ViewModel.TaxiModels
{
    public class CreateBookingViewModel
    {
        [Required]
        public int TaxiCompanyId { get; set; }

        [Required]
        [StringLength(100)]
        public string PickupLocation { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string DropoffLocation { get; set; } = string.Empty;

        [Required]
        public DateTime BookingTime { get; set; }

        [Required]
        public int PassangerCount { get; set; }

        [Required]
        public int UserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}