namespace pentasharp.ViewModel.Dashboard
{
    /// <summary>
    /// Represents the data structure for dashboard metrics.
    /// </summary>
    public class DashboardViewModel
    {
        /// <summary>
        /// The total number of users in the system.
        /// </summary>
        public int TotalUsers { get; set; }

        /// <summary>
        /// The total number of taxi drivers.
        /// </summary>
        public int TaxiDrivers { get; set; }

        /// <summary>
        /// The total number of buses.
        /// </summary>
        public int Buses { get; set; }

        /// <summary>
        /// The total number of taxi companies.
        /// </summary>
        public int TaxiCompanies { get; set; }

        /// <summary>
        /// The total number of bus companies.
        /// </summary>
        public int BusCompanies { get; set; }

        /// <summary>
        /// The total number of bus reservations.
        /// </summary>
        public int BusReservations { get; set; }

        /// <summary>
        /// The total number of taxi bookings.
        /// </summary>
        public int TaxiBookings { get; set; }

        /// <summary>
        /// The total number of taxi reservations.
        /// </summary>
        public int TaxiReservations { get; set; }

        /// <summary>
        /// The total number of bus schedules.
        /// </summary>
        public int BusSchedules { get; set; }
    }
}