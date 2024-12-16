namespace pentasharp.ViewModel.BusReservation
{
    /// <summary>
    /// Represents a bus reservation made by a user.
    /// </summary>
    public class MyBusReservationViewModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the reservation.
        /// </summary>
        public int ReservationId { get; set; }

        /// <summary>
        /// Gets or sets the date when the reservation was made.
        /// </summary>
        public DateTime ReservationDate { get; set; }

        /// <summary>
        /// Gets or sets the number of seats reserved.
        /// </summary>
        public int NumberOfSeats { get; set; }

        /// <summary>
        /// Gets or sets the total amount paid for the reservation.
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Gets or sets the payment status of the reservation.
        /// </summary>
        public string PaymentStatus { get; set; }

        /// <summary>
        /// Gets or sets the current status of the reservation (e.g., confirmed, canceled).
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the bus number for the reservation.
        /// </summary>
        public int BusNumber { get; set; }

        /// <summary>
        /// Gets or sets the departure location of the bus.
        /// </summary>
        public string FromLocation { get; set; }

        /// <summary>
        /// Gets or sets the arrival location of the bus.
        /// </summary>
        public string ToLocation { get; set; }

        /// <summary>
        /// Gets or sets the departure time for the reservation.
        /// </summary>
        public DateTime DepartureTime { get; set; }

        /// <summary>
        /// Gets or sets the arrival time for the reservation.
        /// </summary>
        public DateTime ArrivalTime { get; set; }

        /// <summary>
        /// Gets or sets the price per seat for the reservation.
        /// </summary>
        public decimal Price { get; set; }
    }
}