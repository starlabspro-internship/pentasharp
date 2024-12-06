using System;

namespace pentasharp.ViewModel.TaxiModels
{
    /// <summary>
    /// ViewModel for displaying taxi company details.
    /// </summary>
    public class TaxiCompanyViewModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the taxi company.
        /// </summary>
        public int TaxiCompanyId { get; set; }

        /// <summary>
        /// Gets or sets the name of the taxi company.
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the contact information for the taxi company.
        /// </summary>
        public string ContactInfo { get; set; }

        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the taxi company was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
