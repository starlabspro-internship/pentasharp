namespace pentasharp.ViewModel.TaxiModels
{
    public class DriverViewModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int? CompanyId { get; set; } 
        public string? CompanyName { get; set; } 
    }
}