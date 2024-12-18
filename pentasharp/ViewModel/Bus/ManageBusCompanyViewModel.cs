using System.Collections.Generic;

namespace pentasharp.ViewModel.Bus
{
    /// <summary>
    /// Represents a view model used to manage and display a list of bus companies.
    /// </summary>
    public class ManageBusCompanyViewModel
    {
        /// <summary>
        /// A collection of bus companies.
        /// </summary>
        public List<BusCompanyViewModel> BusCompanies { get; set; } = new List<BusCompanyViewModel>();
    }
}