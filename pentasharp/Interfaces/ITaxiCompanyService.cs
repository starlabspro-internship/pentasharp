using pentasharp.ViewModel.TaxiModels;
using pentasharp.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using pentasharp.Models.TaxiRequest;

namespace pentasharp.Services
{
    /// <summary>
    /// Provides functionality for managing taxi companies in the application.
    /// </summary>
    public interface ITaxiCompanyService
    {
        /// <summary>
        /// Retrieves a list of all taxi companies.
        /// </summary>
        List<TaxiCompanyRequest> GetAllCompanies();

        /// <summary>
        /// Adds a new taxi company to the system.
        /// </summary>
        Task<TaxiCompany> AddCompanyAsync(TaxiCompanyRequest model);

        /// <summary>
        /// Retrieves a taxi company by its unique identifier.
        /// </summary>
        TaxiCompany GetCompanyById(int id);

        /// <summary>
        /// Edits the details of an existing taxi company.
        /// </summary>
        Task<bool> EditCompanyAsync(int id, TaxiCompanyRequest model);

        /// <summary>
        /// Deletes a taxi company by marking it as deleted in the system.
        /// </summary>
        Task<bool> DeleteCompanyAsync(int id);
    }
}