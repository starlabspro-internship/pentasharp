using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using pentasharp.Models.Entities;

namespace pentasharp.ViewModel.BusSchedul
{
    /// <summary>
    /// Represents the data required to add a new bus route.
    /// </summary>
    public class AddRouteViewModel
    {
        /// <summary>
        /// Unique identifier for the route.
        /// </summary>
        public int RouteId { get; set; }

        /// <summary>
        /// Starting location of the route.
        /// </summary>
        public string FromLocation { get; set; } = string.Empty;

        /// <summary>
        /// Destination location of the route.
        /// </summary>
        public string ToLocation { get; set; } = string.Empty;

        /// <summary>
        /// Estimated duration of the route.
        /// </summary>
        public TimeSpan EstimatedDuration { get; set; }
    }
}