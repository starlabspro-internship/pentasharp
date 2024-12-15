using pentasharp.Models.Enums;

namespace pentasharp.Models.BusRequests
{
    public class AddBusReservationRequest
    {
        /// <summary>
        /// Unique identifier for the reservation.
        /// </summary>
        public int ReservationId { get; set; }

        /// <summary>
        /// The date and time when the reservation was made.
        /// </summary>
        public DateTime ReservationDate { get; set; }

        /// <summary>
        /// The number of seats reserved.
        /// </summary>
        public int NumberOfSeats { get; set; }

        /// <summary>
        /// The total amount paid for the reservation.
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Unique identifier of the bus schedule associated with this reservation.
        /// </summary>
        public int ScheduleId { get; set; }

        /// <summary>
        /// Unique identifier of the user who made the reservation.
        /// </summary>
        public int UserId { get; set; }

        public int BusCompanyId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// Current status of the reservation.
        /// </summary>
        public BusReservationStatus Status { get; set; } = BusReservationStatus.Pending;
    }
}