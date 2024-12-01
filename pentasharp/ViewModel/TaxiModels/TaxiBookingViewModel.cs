namespace pentasharp.ViewModel.TaxiModels
{
    public class TaxiBookingViewModel
    {
        public int BookingId { get; set; }
        public string PassengerName { get; set; }
        public string PickupLocation { get; set; }
        public string DropoffLocation { get; set; }
        public string BookingTime { get; set; }
        public string Status { get; set; }
        public string DriverName { get; set; } = "No Driver Assign";
        public int? TaxiId { get; set; }
    }
}