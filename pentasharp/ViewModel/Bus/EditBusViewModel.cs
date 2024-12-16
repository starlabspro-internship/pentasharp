using Microsoft.AspNetCore.Mvc.Rendering;
using pentasharp.Models.Entities;
using pentasharp.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace pentasharp.ViewModel.Bus
{
    /// <summary>
    /// ViewModel for editing bus details.
    /// </summary>
    public class EditBusViewModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the bus.
        /// </summary>
        public int BusId { get; set; }
        /// <summary>
        /// Gets or sets the bus number.
        /// </summary>
        public int BusNumber { get; set; }
        /// <summary>
        /// Gets or sets the status of the bus.
        /// </summary>
        public BusStatus Status { get; set; }
        /// <summary>
        /// Gets or sets the seating capacity of the bus.
        /// </summary>
        public int Capacity { get; set; }
        /// <summary>
        /// Gets the available options for bus status as a select list.
        /// </summary>
        public SelectList StatusOptions => new SelectList(Enum.GetValues(typeof(BusStatus)));
        /// <summary>
        /// Gets or sets the identifier for the bus company.
        /// </summary>
        public int BusCompanyId { get; set; }
        /// <summary>
        /// Gets or sets the date and time when the bus information was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; }
        /// <summary>
        /// Gets or sets the list of bus companies associated with this bus.
        /// </summary>
        public List<BusCompany> BusCompanies { get; set; } = new List<BusCompany>();
    }
}