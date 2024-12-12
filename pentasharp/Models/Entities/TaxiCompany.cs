using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pentasharp.Models.Entities
{
    /// <summary>
    /// Represents a taxi company, including its details and associated taxis.
    /// </summary>
    public class TaxiCompany
    {
        /// <summary>
        /// Unique identifier for the taxi company.
        /// </summary>
        [Key]
        public int TaxiCompanyId { get; set; }

        /// <summary>
        /// Name of the taxi company.
        /// </summary>
        public string CompanyName { get; set; } = string.Empty;

        /// <summary>
        /// Contact information for the taxi company.
        /// </summary>
        public string ContactInfo { get; set; } = string.Empty;

        /// <summary>
        /// Identifier of the user who owns or manages the taxi company.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Date and time when the taxi company was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date and time when the taxi company was last updated. Nullable if no updates have been made.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Collection of taxi bookings associated with the taxi company.
        /// </summary>
        public ICollection<TaxiBookings> TaxiBookings { get; set; } = new List<TaxiBookings>();

        /// <summary>
        /// Collection of taxis associated with the taxi company.
        /// </summary>
        public ICollection<Taxi> Taxis { get; set; } = new List<Taxi>();

        /// <summary>
        /// The collection of taxi reservations associated with the taxi company.
        /// </summary>
        public ICollection<TaxiReservations> TaxiReservations { get; set; } = new List<TaxiReservations>();

        /// <summary>
        /// Indicates whether the taxi company is marked as deleted.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// User who owns or manages the taxi company.
        /// </summary>
        public User User { get; set; } = null!;
    }
}