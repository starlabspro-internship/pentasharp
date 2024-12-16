using pentasharp.Models.Entities;
using System.Collections.Generic;

namespace pentasharp.ViewModel.Bus
{
    /// <summary>
    /// ViewModel for adding a new bus company and associated buses.
    /// </summary>
    public class AddBusCompanyViewModel
    {   /// <summary>
        /// Gets or sets the name of the bus company.
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// Gets or sets the contact information for the bus company.
        /// </summary>
        public string ContactInfo { get; set; }
        /// <summary>
        /// Gets or sets the list of existing bus companies.
        /// </summary>
        public List<BusCompany> BusCompanies { get; set; }
        /// <summary>
        /// Gets or sets the list of buses associated with the company.
        /// </summary>
        public List<Buses> Buses { get; set; }
        /// <summary>
        /// Gets or sets the ViewModel for adding a new bus.
        /// </summary>
        public AddBusViewModel AddBusViewModel { get; set; }

    }
}