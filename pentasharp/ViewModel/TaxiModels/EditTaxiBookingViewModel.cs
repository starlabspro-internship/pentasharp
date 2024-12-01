using pentasharp.Models.Enums;

namespace pentasharp.ViewModel.TaxiModels
{
    public class EditTaxiBookingViewModel
    {
        public int BookingId { get; set; }
        public string PickupLocation { get; set; }
        public string DropoffLocation { get; set; }
        public DateTime BookingTime { get; set; }
        public string Status { get; set; }
        public int? DriverId { get; set; }
        public int? TaxiId { get; set; }
        public DateTime UpdateAd { get; set; } = DateTime.Now;
    }
}