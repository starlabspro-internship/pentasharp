namespace pentasharp.Models.TaxiRequest
{
    /// <summary>
    /// Represents a DTO for a taxi company, including its details.
    /// </summary>
    public class TaxiCompanyRequest
    {
        /// <summary>
        /// Unique identifier for the taxi company.
        /// </summary>
        public int TaxiCompanyId { get; set; }

        /// <summary>
        /// Name of the taxi company.
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Contact information for the taxi company.
        /// </summary>
        public string ContactInfo { get; set; }

        /// <summary>
        /// User ID associated with the taxi company.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Date and time when the taxi company was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}