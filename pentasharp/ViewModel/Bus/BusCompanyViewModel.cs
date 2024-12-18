namespace pentasharp.ViewModel.Bus
{/// <summary>
 /// ViewModel for displaying bus company details.
 /// </summary>
    public class BusCompanyViewModel
    {  /// <summary>
       /// Gets or sets the unique identifier for the bus company.
       /// </summary>
        public int BusCompanyId { get; set; }
        /// <summary>
        /// Gets or sets the name of the bus company.
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// Gets or sets the contact information for the bus company.
        /// </summary>
        public string ContactInfo { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the user who owns the bus company.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the bus company was created.
        /// </summary>
        /// 
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}