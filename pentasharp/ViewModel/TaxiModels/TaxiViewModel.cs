using System.Globalization;
using pentasharp.Models.Enums; 

namespace pentasharp.ViewModel.Taxi
{
    public class TaxiViewModel
    {
        public int TaxiId { get; set; }
        public string LicensePlate { get; set; }
        public string DriverName { get; set; }
        public int TaxiCompanyId { get; set; }
        public string CompanyName { get; set; }
        public string Status { get; set; }
    }
}
