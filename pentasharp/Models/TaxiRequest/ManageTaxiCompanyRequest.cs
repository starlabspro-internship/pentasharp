using pentasharp.ViewModel.TaxiModels;

namespace pentasharp.Models.TaxiRequest
{
    /// <summary>
    /// Represents the data required to manage taxi companies.
    /// </summary>
    public class ManageTaxiCompanyRequest
    {
        /// <summary>
        /// List of taxi companies to be managed.
        /// </summary>
        public List<TaxiCompanyRequest> TaxiCompanies { get; set; } = new List<TaxiCompanyRequest>();
    }
}