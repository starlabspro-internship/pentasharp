using System.ComponentModel.DataAnnotations;

namespace pentasharp.Models.Entities
{
    /// <summary>
    /// Represents a specific bus route within the transportation network.
    /// </summary>
    public class BusRoutes
    {
        /// <summary>
        /// Gets or sets the unique identifier for the bus route.
        /// </summary>
        [Key]
        public int RouteId { get; set; }
    }
}
