using pentasharp.Models.Entities;
using System.Collections.Generic;

namespace pentasharp.ViewModel.Bus
{
    public class AddBusCompanyViewModel
    {
        public string CompanyName { get; set; }
        public string ContactInfo { get; set; }

        public List<BusCompany> BusCompanies { get; set; } // List of companies for the dropdown
        public List<Buses> Buses { get; set; }
        public AddBusViewModel AddBusViewModel { get; set; }

    }
}
