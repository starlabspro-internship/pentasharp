namespace pentasharp.Models.TaxiRequest
{
    /// <summary>
    /// Represents the data required to register a new driver.
    /// </summary>
    public class RegisterDriverRequest
    {
        /// <summary>
        /// First name of the driver.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name of the driver.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Email address of the driver.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Password for the driver's account.
        /// </summary>
        public string Password { get; set; }
    }
}