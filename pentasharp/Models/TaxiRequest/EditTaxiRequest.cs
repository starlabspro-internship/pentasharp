using pentasharp.Models.Entities;

namespace pentasharp.Models.TaxiRequest
{
    /// <summary>
    /// Represents the data required to edit a taxi's details.
    /// </summary>
    public class EditTaxiRequest
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
        /// ID of the driver associated with the taxi, if any.
        /// </summary>
        public int? DriverId { get; set; }

        /// <summary>
        /// Identifier for the taxi company to which the taxi belongs.
        /// </summary>
        public int TaxiCompanyId { get; set; }

        /// <summary>
        /// Date and time when the taxi information was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// List of taxi companies associated with the taxi.
        /// </summary>
        public List<TaxiCompany> TaxiCompanies { get; set; } = new List<TaxiCompany>();
    }
}