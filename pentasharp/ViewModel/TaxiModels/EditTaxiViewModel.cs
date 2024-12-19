using Microsoft.AspNetCore.Mvc.Rendering;
using pentasharp.Models.Entities;
using pentasharp.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace pentasharp.ViewModel.TaxiModels
{
    /// <summary>
    /// ViewModel for editing taxi details.
    /// </summary>
    public class EditTaxiViewModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the taxi.
        /// </summary>
        public int TaxiId { get; set; }

        /// <summary>
        /// Gets or sets the license plate.
        /// </summary>
        public string LicensePlate { get; set; }

        /// <summary>
        /// Gets or sets the driver's name.
        /// </summary>
        public int? DriverId { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the taxi company.
        /// </summary>
        public int TaxiCompanyId { get; set; }

        /// <summary>s
        /// Gets or sets the date and time when the taxi information was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the list of taxi companies associated with this taxi.
        /// </summary>
        public List<TaxiCompany> TaxiCompanies { get; set; } = new List<TaxiCompany>();
    }
}
