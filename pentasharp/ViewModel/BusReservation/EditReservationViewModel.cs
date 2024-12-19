using pentasharp.Models.Enums;

namespace pentasharp.ViewModel.BusReservation
{
    /// <summary>
    /// Represents the data required to edit an existing bus reservation.
    /// </summary>
    public class EditReservationViewModel
    {
        /// <summary>
        /// Unique identifier for the reservation to be edited.
        /// </summary>
        public int ReservationId { get; set; }

        /// <summary>
        /// New status for the reservation.
        /// </summary>
        public BusReservationStatus Status { get; set; }

        /// <summary>
        /// The time when the reservation was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
    }
}