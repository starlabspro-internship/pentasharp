using System.Globalization;

namespace pentasharp.ViewModel.Bus
{
    public class BusViewModel
    {
        public int BusId { get; set; }
        public int BusNumber { get; set; }
        public int Capacity { get; set; }
        public int BusCompanyId { get; set; }
        public string CompanyName { get; set; }
    }
}