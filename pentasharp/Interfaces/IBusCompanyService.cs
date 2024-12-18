using pentasharp.Models.DTOs;
using pentasharp.ViewModel.Bus;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pentasharp.Interfaces
{
    /// <summary>
    /// Provides functionality for managing bus companies.
    /// </summary>
    public interface IBusCompanyService
    {
        /// <summary>
        /// Retrieves all bus companies.
        /// </summary>
        Task<List<BusCompanyViewModel>> GetCompaniesAsync();

        /// <summary>
        /// Adds a new bus company.
        /// </summary>
        Task<bool> AddCompanyAsync(BusCompanyViewModel model);

        /// <summary>
        /// Edits an existing bus company.
        /// </summary>
        Task<bool> EditCompanyAsync(int id, BusCompanyViewModel model);

        /// <summary>
        /// Deletes an existing bus company.
        /// </summary>
        Task<bool> DeleteCompanyAsync(int id);

        /// <summary>
        /// Retrieves the user associated with a specific bus company.
        /// </summary>
        Task<StandardApiResponse<object>> GetBusCompanyUserAsync(int companyId);

        /// <summary>
        /// Retrieves a list of users that belong to bus companies.
        /// </summary>
        Task<List<object>> GetBusCompanyUsersAsync();

        /// <summary>
        /// Retrieves a company by a specific user identifier.
        /// </summary>
        Task<object> GetCompanyByUserIdAsync();

        List<BusCompanyViewModel> GetAllBusCompanies();
    }
}