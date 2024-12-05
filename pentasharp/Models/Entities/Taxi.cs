using System;
using System.Collections.Generic;
using pentasharp.Models.Enums;

namespace pentasharp.Models.Entities
{
    /// <summary>
    /// Represents a taxi vehicle within the system.
    /// </summary>
    public class Taxi
    {
        /// <summary>
        /// Gets or sets the unique identifier for the taxi.
        /// </summary>
        public int TaxiId { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the associated taxi company.
        /// </summary>
        public int TaxiCompanyId { get; set; }

        /// <summary>
        /// Gets or sets the license plate of the taxi.
        /// </summary>
        public string LicensePlate { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the taxi driver.
        /// </summary>
        public string DriverName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the current status of the taxi.
        /// </summary>
        public TaxiStatus Status { get; set; } = TaxiStatus.Available;

        /// <summary>
        /// Gets or sets the creation date and time of the taxi record.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the last updated date and time for the taxi record, if any.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the associated taxi company.
        /// </summary>
        public TaxiCompany TaxiCompany { get; set; } = null!;

        /// <summary>
        /// Gets or sets the collection of taxi reservations associated with this taxi.
        /// </summary>
        public ICollection<TaxiReservations> TaxiReservations { get; set; } = new List<TaxiReservations>();

        /// <summary>
        /// Gets or sets the collection of taxi bookings associated with this taxi.
        /// </summary>
        public ICollection<TaxiBookings> TaxiBookings { get; set; } = new List<TaxiBookings>();
    }
}
