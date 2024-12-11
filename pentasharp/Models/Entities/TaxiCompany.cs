using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pentasharp.Models.Entities
{
    public class TaxiCompany
    {
        /// <summary>
        /// Gets or sets the unique identifier for the taxi company.
        /// </summary>
        [Key]
        public int TaxiCompanyId { get; set; }

        /// <summary>
        /// Gets or sets the name of the taxi company.
        /// </summary>
        public string CompanyName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the contact information for the taxi company.
        /// </summary>
        public string ContactInfo { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date and time when the taxi company was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the date and time when the taxi company was last updated.
        /// Nullable to allow for no updates.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the collection of taxis associated with the taxi company.
        /// </summary>
        public ICollection<Taxi> Taxis { get; set; } = new List<Taxi>();

        /// <summary>
        /// The collection of taxi reservations associated with the taxi company.
        /// </summary>
        public ICollection<TaxiReservations> TaxiReservations { get; set; } = new List<TaxiReservations>();

        /// <summary>
        /// The collection of taxi bookings associated with the taxi company.
        /// </summary>
        public ICollection<TaxiBookings> TaxiBookings { get; set; } = new List<TaxiBookings>();

        /// <summary>
        /// Gets or sets a value indicating whether the taxi company is marked as deleted.
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}