namespace pentasharp.Models.TaxiRequest
{
    /// <summary>
    /// Represents a DTO for a taxi company, including details about the company.
    /// </summary>
    public class TaxiCompanyRequest
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
    }
}