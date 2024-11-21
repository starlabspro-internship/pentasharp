namespace pentasharp.ViewModel.TaxiReservation
{
    public class TaxiSearchViewModel
    {
        public string PickupLocation { get; set; }
        public string DropoffLocation { get; set; }
        public DateTime ReservationDate { get; set; }
        public TimeSpan ReservationTime { get; set; }
        public int PassengerCount { get; set; }


    }
}
