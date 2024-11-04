using System;
using System.ComponentModel.DataAnnotations;
using pentasharp.Models.Enums;

namespace pentasharp.Models.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(256)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public UserRole Role { get; set; } = UserRole.User;

        [Required]
        public bool IsAdmin { get; set; } = false;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<TaxiReservations> TaxiReservations { get; set; } = new List<TaxiReservations>();

        public ICollection<TaxiBookings> TaxiBookings { get; set; } = new List<TaxiBookings>();
        public ICollection<Notifications> Notifications { get; set; } = new List<Notifications>();

    }
}
