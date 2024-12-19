using pentasharp.Models.Enums;

namespace pentasharp.Models.TaxiRequest
{
    /// <summary>
    /// Represents a DTO for a taxi, including its details.
    /// </summary>
    public class TaxiRequest
    {
        /// <summary>
        /// Unique identifier for the taxi.
        /// </summary>
        public int TaxiId { get; set; }

        /// <summary>
        /// License plate of the taxi.
        /// </summary>
        public string LicensePlate { get; set; }

        /// <summary>
        /// Name of the driver assigned to the taxi.
        /// </summary>
        public string DriverName { get; set; }

        /// <summary>
        /// ID of the driver assigned to the taxi, if any.
        /// </summary>
        public int? DriverId { get; set; }

        /// <summary>
        /// Identifier of the taxi company to which the taxi belongs.
        /// </summary>
        public int TaxiCompanyId { get; set; }

        /// <summary>
        /// Name of the taxi company to which the taxi belongs.
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Status of the taxi, represented by the <see cref="TaxiStatus"/> enum.
        /// </summary>
        public TaxiStatus Status { get; set; }
    }
}