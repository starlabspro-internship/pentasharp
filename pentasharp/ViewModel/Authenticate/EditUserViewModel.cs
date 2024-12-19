using pentasharp.Models.Enums;

namespace pentasharp.ViewModel.Authenticate
{
    /// <summary>
    /// Represents the data required to edit an existing user's information.
    /// </summary>
    public class EditUserViewModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the user's first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the user's last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the user's email address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the user's password.
        /// </summary>
        public string Password { get; set; }

        public int? CompanyId { get; set; }

        /// <summary>
        /// Gets or sets the user's role.
        /// </summary>
        public UserRole? Role { get; set; }

        /// <summary>
        /// Gets or sets the user's business type (e.g., BusCompany, TaxiCompany).
        /// </summary>
        public BusinessType? BusinessType { get; set; }
    }
}