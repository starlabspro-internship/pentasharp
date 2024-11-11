using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using pentasharp.Models.Entities;

namespace pentasharp.ViewModel.Bus
{
    public class AddBusViewModel
    {
        public int BusId { get; set; }

        [Required]
        public int? BusNumber { get; set; }

        [Required]
        public int? Capacity { get; set; }
        [Required]
        public int? BusCompanyId { get; set; }  // The selected bus company ID

        public BusCompany? BusCompany { get; set; }
        public List<BusCompany> BusCompanies { get; set; } = new List<BusCompany>();  // List of bus companies for the dropdown

    }
}
