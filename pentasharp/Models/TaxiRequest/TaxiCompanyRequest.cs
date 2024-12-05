namespace pentasharp.Models.TaxiRequest
{
    /// <summary>
    /// Represents a model for taxi company operations (create, update, get).
    /// </summary>
    public class TaxiCompanyRequest
    {
        /// <summary>
        /// The name of the taxi company.
        /// </summary>
        public string CompanyName { get; set; } = string.Empty;

        /// <summary>
        /// The contact information for the taxi company.
        /// </summary>
        public string ContactInfo { get; set; } = string.Empty;
    }
}
