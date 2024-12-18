namespace pentasharp.Models.DTOs
{
    /// <summary>
    /// Represents a data transfer object for an admin user with essential information.
    /// </summary>
    public class AdminUserDto
    {
        /// <summary>
        /// The first name of the admin user.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the admin user.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The email address of the admin user.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The password for the admin user.
        /// </summary>
        public string Password { get; set; } 
    }
}