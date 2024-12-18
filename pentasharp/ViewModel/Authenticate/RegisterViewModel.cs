using System;
using System.ComponentModel.DataAnnotations;
using pentasharp.Models.Entities;
using pentasharp.Models.Enums;

namespace pentasharp.ViewModel.Authenticate
{
    /// <summary>
    /// Represents the registration data required for a new user.
    /// </summary>
    public class RegisterViewModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the user's first name.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the user's last name.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the user's email address.
        /// </summary>
        [Required]
        [EmailAddress]
        [StringLength(256)]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the user's password.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the confirmation password, which must match the Password field.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        public bool IsAdmin { get; set; } = false;

        public UserRole? Role { get; set; }
        public BusinessType BusinessType { get; set; } = BusinessType.None;
    }
}
