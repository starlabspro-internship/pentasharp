using System.ComponentModel.DataAnnotations;

namespace pentasharp.ViewModel.Authenticate
{
    /// <summary>
    /// Represents the data required for user login.
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// The user's email address.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// The user's password.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
