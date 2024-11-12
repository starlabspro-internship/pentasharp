using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using pentasharp.Models.Entities;

namespace pentasharp.ViewModel.Bus
{  /// <summary>
   /// ViewModel for adding a new bus.
   /// </summary>
    public class AddBusViewModel
    { /// <summary>
      /// Gets or sets the unique identifier for the bus.
      /// </summary>
        public int BusId { get; set; }
        /// <summary>
        /// Gets or sets the bus number (required).
        /// </summary>
        [Required]
        public int? BusNumber { get; set; }
        /// <summary>
        /// Gets or sets the seating capacity of the bus (required).
        /// </summary>
        [Required]
        public int? Capacity { get; set; }
        /// <summary>
        /// Gets or sets the identifier for the bus company (required).
        /// </summary>
        [Required]
        public int? BusCompanyId { get; set; }
        /// <summary>
        /// Gets or sets the bus company associated with this bus.
        /// </summary>
        public BusCompany? BusCompany { get; set; }
        /// <summary>
        /// Gets or sets the list of available bus companies.
        /// </summary>
        public List<BusCompany> BusCompanies { get; set; } = new List<BusCompany>();
    }
}