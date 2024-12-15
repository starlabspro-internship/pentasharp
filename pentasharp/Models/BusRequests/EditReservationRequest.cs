using pentasharp.Models.Enums;

namespace pentasharp.Models.BusRequests
{
    public class EditReservationRequest
    {
        /// <summary>
        /// Unique identifier for the reservation to be edited.
        /// </summary>
        public int ReservationId { get; set; }

        /// <summary>
        /// New status for the reservation.
        /// </summary>
        public BusReservationStatus Status { get; set; }

        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
    }
}