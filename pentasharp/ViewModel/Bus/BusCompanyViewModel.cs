namespace pentasharp.ViewModel.Bus
{
    public class BusCompanyViewModel
    {
        public int BusCompanyId { get; set; }
        public string CompanyName { get; set; }
        public string ContactInfo { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
