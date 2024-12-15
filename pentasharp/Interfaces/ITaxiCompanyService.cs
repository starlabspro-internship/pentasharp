using pentasharp.Models.TaxiRequest;
using pentasharp.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pentasharp.Services
{
    /// <summary>
    /// Provides functionality for managing taxi companies in the application.
    /// </summary>
    public interface ITaxiCompanyService
    {
        /// <summary>
        /// Retrieves a list of all taxi companies (basic).
        /// </summary>
        List<TaxiCompanyRequest> GetAllCompanies();

        /// <summary>
        /// Retrieves all taxi companies along with their taxis.
        /// </summary>
        List<TaxiCompanyRequest> GetAllCompaniesWithTaxis();

        /// <summary>
        /// Adds a new taxi company to the system.
        /// </summary>
        Task<TaxiCompany> AddCompanyAsync(TaxiCompanyRequest model);

        /// <summary>
        /// Adds a new company and assigns a user to it, if provided.
        /// </summary>
        Task<bool> AddCompanyAndAssignUserAsync(TaxiCompanyRequest model);

        /// <summary>
        /// Retrieves a taxi company by its unique identifier.
        /// </summary>
        TaxiCompany GetCompanyById(int id);

        /// <summary>
        /// Edits the details of an existing taxi company.
        /// </summary>
        Task<bool> EditCompanyAsync(int id, TaxiCompanyRequest model);

        /// <summary>
        /// Edits a company and assigns a user to it, if provided.
        /// </summary>
        Task<bool> EditCompanyAndAssignUserAsync(int id, TaxiCompanyRequest model);

        /// <summary>
        /// Deletes a taxi company by marking it as deleted in the system.
        /// </summary>
        Task<bool> DeleteCompanyAsync(int id);

        /// <summary>
        /// Retrieves a list of users that are TaxiCompany type and have no assigned CompanyId.
        /// </summary>
        List<object> GetUnassignedTaxiCompanyUsers();

        /// <summary>
        /// Retrieves the user assigned to a particular company.
        /// Returns null if the company does not exist.
        /// </summary>
        object GetTaxiCompanyUser(int companyId);
    }
}