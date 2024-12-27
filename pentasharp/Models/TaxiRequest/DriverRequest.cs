namespace pentasharp.Models.TaxiRequest
{
    /// <summary>
    /// Represents the data for a driver in the system.
    /// </summary>
    public class DriverRequest
    {
        /// <summary>
        /// Unique identifier for the driver.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// First name of the driver.
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Last name of the driver.
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Email address of the driver.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Identifier of the company associated with the driver, if any.
        /// </summary>
        public int? CompanyId { get; set; }

        /// <summary>
        /// Name of the company associated with the driver, if any.
        /// </summary>
        public string? CompanyName { get; set; }
    }
}