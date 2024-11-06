using System.ComponentModel.DataAnnotations;

namespace pentasharp.Models.Entities
{
    /// <summary>
    /// Represents a bus in the transportation network.
    /// </summary>
    public class Buses
    {
        /// <summary>
        /// Gets or sets the unique identifier for the bus.
        /// </summary>
        [Key]
        public int BusId { get; set; }
    }
}
