namespace pentasharp.ViewModel.BusSchedul
{
    /// <summary>
    /// Represents the data returned when searching for bus schedules.
    /// </summary>
    public class SearchScheduleViewModel
    {
        /// <summary>
        /// Unique identifier for the schedule.
        /// </summary>
        public int ScheduleId { get; set; }

        /// <summary>
        /// Departure location of the route.
        /// </summary>
        public string FromLocation { get; set; } = null!;

        /// <summary>
        /// Arrival location of the route.
        /// </summary>
        public string ToLocation { get; set; } = null!;

        /// <summary>
        /// Departure time of the bus schedule.
        /// </summary>
        public DateTime DepartureTime { get; set; }

        /// <summary>
        /// Arrival time of the bus schedule.
        /// </summary>
        public DateTime ArrivalTime { get; set; }

        /// <summary>
        /// Bus number assigned to the schedule.
        /// </summary>
        public int BusNumber { get; set; }

        /// <summary>
        /// Price of the ticket for the schedule.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Number of available seats for the schedule.
        /// </summary>
        public int AvailableSeats { get; set; }

        /// <summary>
        /// Unique identifier for the bus company that owns this schedule.
        /// </summary>
        public int BusCompanyId { get; set; }

        /// <summary>
        /// Status of the schedule (e.g., Scheduled, Canceled).
        /// </summary>
        public string Status { get; set; } = null!;
    }
}