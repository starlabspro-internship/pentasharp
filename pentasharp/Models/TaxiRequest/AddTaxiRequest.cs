using pentasharp.Models.Entities;
using pentasharp.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace pentasharp.Models.TaxiRequest
{
    /// <summary>
    /// Represents the data required to add a new taxi to the system.
    /// </summary>
    public class AddTaxiRequest
    {
        /// <summary>
        /// Unique identifier for the taxi.
        /// </summary>
        public int TaxiId { get; set; }

        /// <summary>
        /// License plate of the taxi (required).
        /// </summary>
        public string LicensePlate { get; set; }

        /// <summary>
        /// Driver's ID associated with the taxi (required).
        /// </summary>
        public int? DriverId { get; set; }

        /// <summary>
        /// Identifier for the taxi company (required).
        /// </summary>
        public int? TaxiCompanyId { get; set; }

        /// <summary>
        /// Taxi company associated with this taxi.
        /// </summary>
        public TaxiCompany? TaxiCompany { get; set; }

        /// <summary>
        /// List of available taxi companies.
        /// </summary>
        public List<TaxiCompany> TaxiCompanies { get; set; } = new List<TaxiCompany>();

        /// <summary>
        /// Status of the taxi using the TaxiStatus enum.
        /// </summary>
        public TaxiStatus Status { get; set; } = TaxiStatus.Available;
    }
}