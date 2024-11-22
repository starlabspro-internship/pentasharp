using pentasharp.Models.Enums;

namespace pentasharp.ViewModel.BusReservation
{
    public class EditReservationViewModel
    {
        public int ReservationId { get; set; }
        public BusReservationStatus Status { get; set; }
    }
}
