using Microsoft.AspNetCore.Mvc.Rendering;
using pentasharp.Models.Entities;
using pentasharp.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace pentasharp.ViewModel.Bus
{
            public class EditBusViewModel
        {
            public int BusId { get; set; }

          
            public int BusNumber { get; set; }

        public BusStatus Status { get; set; }

        public int Capacity { get; set; }

        public SelectList StatusOptions => new SelectList(Enum.GetValues(typeof(BusStatus)));
        public int BusCompanyId { get; set; }  // The selected bus company ID


        public DateTime UpdatedAt { get; set; }

        public List<BusCompany> BusCompanies { get; set; } = new List<BusCompany>();
        
    }
}
