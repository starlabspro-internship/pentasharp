namespace pentasharp.Models.TaxiRequest
{
    /// <summary>
    /// Represents the data required to add a taxi driver to the system.
    /// </summary>
    public class AddTaxiDriverRequest
    {
        /// <summary>
        /// The ID of the taxi company to which the driver will be assigned.
        /// </summary>
        public int TaxiCompanyId { get; set; }

        /// <summary>
        /// The license plate of the taxi associated with the driver.
        /// </summary>
        public string LicensePlate { get; set; }

        /// <summary>
        /// The name of the driver.
        /// </summary>
        public string DriverName { get; set; }

        /// <summary>
        /// The email address of the driver.
        /// </summary>
        public string DriverEmail { get; set; }

        /// <summary>
        /// The password for the driver's account.
        /// </summary>
        public string DriverPassword { get; set; }
    }
}