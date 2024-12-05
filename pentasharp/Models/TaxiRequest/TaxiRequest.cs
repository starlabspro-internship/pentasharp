using pentasharp.Models.Enums;

namespace pentasharp.Models.TaxiRequest
{
    /// <summary>
    /// Represents a model for taxi operations (create, update, get).
    /// </summary>
    public class TaxiRequest
    {
        /// <summary>
        /// The ID of the taxi company associated with this taxi.
        /// </summary>
        public int TaxiCompanyId { get; set; }

        /// <summary>
        /// The license plate of the taxi.
        /// </summary>
        public string LicensePlate { get; set; } = string.Empty;

        /// <summary>
        /// The name of the taxi driver.
        /// </summary>
        public string DriverName { get; set; } = string.Empty;

        /// <summary>
        /// The status of the taxi.
        /// </summary>
        public TaxiStatus Status { get; set; } = TaxiStatus.Available;
    }
}
