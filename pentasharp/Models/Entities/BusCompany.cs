using System.ComponentModel.DataAnnotations;

namespace pentasharp.Models.Entities
{
    /// <summary>
    /// Represents a bus company entity with details such as name, contact information, and associated buses.
    /// </summary>
    public class BusCompany
    {
        /// <summary>
        /// Gets or sets the unique identifier for a bus company.
        /// </summary>
        [Key]
        public int BusCompanyId { get; set; }

        /// <summary>
        /// Gets or sets the name of the bus company.
        /// </summary>
        [Required]
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the contact information for the bus company.
        /// </summary>
        [Required]
        public string ContactInfo { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the bus company was created in the system.
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the bus company information was last updated, if applicable.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the collection of buses associated with this bus company.
        /// </summary>
        public ICollection<Buses> Buses { get; set; } = new List<Buses>();
    }
}
