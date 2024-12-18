using System;
using System.ComponentModel.DataAnnotations;
using pentasharp.Models.Enums;

namespace pentasharp.Models.Entities
{
    /// <summary>
    /// Represents a user in the system, including personal details, roles, and associations.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Unique identifier for the user.
        /// </summary>
        [Key]
        public int UserId { get; set; }

        /// <summary>
        /// First name of the user.
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Last name of the user.
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Email address of the user.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Hashed password of the user.
        /// </summary>
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>
        /// Role of the user within the application (e.g., User, Admin).
        /// </summary>
        public UserRole Role { get; set; } = UserRole.User;

        /// <summary>
        /// Identifier of the company associated with the user, if any.
        /// </summary>
        public int? CompanyId { get; set; }

        /// <summary>
        /// Business type associated with the user.
        /// </summary>
        public BusinessType BusinessType { get; set; } = BusinessType.None;

        /// <summary>
        /// Indicates whether the user has administrative privileges.
        /// </summary>
        public bool IsAdmin { get; set; } = false;

        /// <summary>
        /// Date and time when the user was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Collection of taxi reservations associated with the user.
        /// </summary>
        public ICollection<TaxiReservations> TaxiReservations { get; set; } = new List<TaxiReservations>();

        /// <summary>
        /// Collection of taxi bookings made by the user.
        /// </summary>
        public ICollection<TaxiBookings> TaxiBookings { get; set; } = new List<TaxiBookings>();

        /// <summary>
        /// Collection of notifications for the user.
        /// </summary>
        public ICollection<Notifications> Notifications { get; set; } = new List<Notifications>();

        /// <summary>
        /// The collection of bus reservations made by the user.
        /// </summary>
        public ICollection<BusReservations> BusReservations { get; init; } = new List<BusReservations>();

        /// <summary>
        /// Gets or sets a value indicating whether the user is marked as deleted.
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}