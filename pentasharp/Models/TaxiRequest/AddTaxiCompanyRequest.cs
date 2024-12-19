using pentasharp.Models.Entities;
using pentasharp.ViewModel.TaxiModels;

namespace pentasharp.Models.TaxiRequest
{
    public class AddTaxiCompanyRequest
    {
        /// <summary>
        /// The name of the taxi company.
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// The contact information for the taxi company.
        /// </summary>
        public string ContactInfo { get; set; }

        /// <summary>
        /// A list of existing taxi companies.
        /// </summary>
        public List<TaxiCompany> TaxiCompanies { get; set; }

        /// <summary>
        /// A list of taxis associated with the company.
        /// </summary>
        public List<pentasharp.Models.Entities.Taxi> Taxis { get; set; }

        /// <summary>
        /// The ViewModel for adding a new taxi.
        /// </summary>
        public AddTaxiViewModel AddTaxiViewModel { get; set; }
    }
}