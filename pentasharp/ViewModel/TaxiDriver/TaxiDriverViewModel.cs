namespace pentasharp.ViewModel.TaxiDriver
{
    public class StartTripViewModel
    {
        public int BookingId { get; set; }
    }

    public class EndTripViewModel
    {
        public int BookingId { get; set; }
        public decimal Fare { get; set; }
    }
}
