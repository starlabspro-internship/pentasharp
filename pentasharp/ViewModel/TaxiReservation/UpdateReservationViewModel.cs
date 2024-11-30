using System.ComponentModel.DataAnnotations;

namespace pentasharp.ViewModel.TaxiReservation
{
    public class UpdateReservationViewModel
    {
        public int? TaxiId { get; set; }
        public string? DriverName { get; set; }
        public decimal? Fare { get; set; }
        public string? PickupLocation { get; set; }
        public string? DropoffLocation { get; set; }
        public DateTime? ReservationDate { get; set; }
        public string? ReservationTime { get; set; } = string.Empty;
        public string? Status { get; set; }
    }
}
