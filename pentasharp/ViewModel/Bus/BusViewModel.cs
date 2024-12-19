using System.Globalization;

namespace pentasharp.ViewModel.Bus
{
    /// <summary>
    /// Represents a bus and its associated company information.
    /// </summary>
    public class BusViewModel
    {
        /// <summary>
        /// Unique identifier for the bus.
        /// </summary>
        public int BusId { get; set; }

        /// <summary>
        /// Number assigned to the bus.
        /// </summary>
        public int BusNumber { get; set; }

        /// <summary>
        /// Maximum number of passengers the bus can carry.
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// Unique identifier for the bus company that owns this bus.
        /// </summary>
        public int BusCompanyId { get; set; }

        /// <summary>
        /// Name of the bus company that owns this bus.
        /// </summary>
        public string CompanyName { get; set; } = string.Empty;
    }
}