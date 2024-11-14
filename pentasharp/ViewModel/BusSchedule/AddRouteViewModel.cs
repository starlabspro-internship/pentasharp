using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using pentasharp.Models.Entities;

namespace pentasharp.ViewModel.BusSchedule
{  
    public class AddRouteViewModel
    {
        public int RouteId { get; set; }

        public string FromLocation { get; set; } = string.Empty;

        public string ToLocation { get; set; } = string.Empty;

        public TimeSpan EstimatedDuration { get; set; }
    }
}