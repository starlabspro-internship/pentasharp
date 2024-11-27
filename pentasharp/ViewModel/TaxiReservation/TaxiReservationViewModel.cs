using System;
using System.ComponentModel.DataAnnotations;

namespace pentasharp.ViewModel.TaxiModels
{
    public class TaxiReservationViewModel
    {
        [Required]
        public int TaxiCompanyId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(256)]
        public string PickupLocation { get; set; } = string.Empty;

        [Required]
        [StringLength(256)]
        public string DropoffLocation { get; set; } = string.Empty;

        [Required]
        public DateTime ReservationDate { get; set; }

        [Required]
        public string ReservationTime { get; set; } = string.Empty;


        [Required]
        public int PassengerCount { get; set; }
    }
}