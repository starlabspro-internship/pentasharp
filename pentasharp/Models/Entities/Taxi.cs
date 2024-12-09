using pentasharp.Models.Enums;
using System;
using System.Collections.Generic;

namespace pentasharp.Models.Entities
{
    public class Taxi
    {
        /// <summary>
        /// Unique identifier for the taxi.
        /// </summary>
        public int TaxiId { get; set; }

        /// <summary>
        /// Foreign key for the taxi company associated with the taxi.
        /// </summary>
        public int TaxiCompanyId { get; set; }

        /// <summary>
        /// License plate of the taxi.
        /// </summary>
        public string LicensePlate { get; set; } = string.Empty;

        /// <summary>
        /// Foreign key for the driver, referencing the User entity. Nullable to allow for unassigned drivers.
        /// </summary>
        public int? DriverId { get; set; }

        /// <summary>
        /// Navigation property for the driver.
        /// </summary>
        public User? Driver { get; set; }

        /// <summary>
        /// Current status of the taxi (e.g., Available, In-Use, etc.).
        /// </summary>
        public TaxiStatus Status { get; set; } = TaxiStatus.Available;

        /// <summary>
        /// Date and time when the taxi was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date and time when the taxi was last updated. Nullable to allow for no updates.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Taxi company associated with this taxi.
        /// </summary>
        public TaxiCompany TaxiCompany { get; set; } = null!;

        /// <summary>
        /// Collection of taxi reservations associated with the taxi.
        /// </summary>
        public ICollection<TaxiReservations> TaxiReservations { get; set; } = new List<TaxiReservations>();

        /// <summary>
        /// Collection of taxi bookings associated with the taxi.
        /// </summary>
        public ICollection<TaxiBookings> TaxiBookings { get; set; } = new List<TaxiBookings>();

        /// <summary>
        /// Indicates whether the taxi is marked as deleted.
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}