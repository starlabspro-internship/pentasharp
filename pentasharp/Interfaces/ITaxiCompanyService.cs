using System.Collections.Generic;
using System.Threading.Tasks;
using pentasharp.Models.TaxiRequest;

namespace pentasharp.Interfaces
{
    /// <summary>
    /// Defines the contract for managing taxi companies and their associated taxis.
    /// </summary>
    public interface ITaxiCompanyService
    {
        /// <summary>
        /// Retrieves a list of all taxi companies.
        /// </summary>
        /// <returns>A list of taxi companies.</returns>
        Task<List<TaxiCompanyRequest>> GetAllCompaniesAsync();

        /// <summary>
        /// Retrieves a specific taxi company by its ID.
        /// </summary>
        /// <param name="id">The ID of the taxi company.</param>
        /// <returns>The details of the taxi company.</returns>
        Task<TaxiCompanyRequest> GetCompanyByIdAsync(int id);

        /// <summary>
        /// Creates a new taxi company.
        /// </summary>
        /// <param name="model">The model containing taxi company details.</param>
        /// <returns>A boolean indicating whether the creation was successful.</returns>
        Task<bool> AddCompanyAsync(TaxiCompanyRequest model);

        /// <summary>
        /// Updates an existing taxi company.
        /// </summary>
        /// <param name="id">The ID of the taxi company to update.</param>
        /// <param name="model">The model containing updated company details.</param>
        /// <returns>A boolean indicating whether the update was successful.</returns>
        Task<bool> UpdateCompanyAsync(int id, TaxiCompanyRequest model);

        /// <summary>
        /// Deletes a taxi company by its ID.
        /// </summary>
        /// <param name="id">The ID of the taxi company to delete.</param>
        /// <returns>A boolean indicating whether the deletion was successful.</returns>
        Task<bool> DeleteCompanyAsync(int id);

        /// <summary>
        /// Retrieves a list of all taxis associated with a specific company.
        /// </summary>
        /// <param name="taxiCompanyId">The ID of the taxi company.</param>
        /// <returns>A list of taxis.</returns>
        Task<List<TaxiRequest>> GetTaxisByCompanyAsync(int taxiCompanyId);

        /// <summary>
        /// Adds a new taxi to a company.
        /// </summary>
        /// <param name="model">The model containing taxi details.</param>
        /// <returns>A boolean indicating whether the addition was successful.</returns>
        Task<bool> AddTaxiAsync(TaxiRequest model);

        /// <summary>
        /// Updates a taxi's details.
        /// </summary>
        /// <param name="taxiId">The ID of the taxi to update.</param>
        /// <param name="model">The model containing updated taxi details.</param>
        /// <returns>A boolean indicating whether the update was successful.</returns>
        Task<bool> UpdateTaxiAsync(int taxiId, TaxiRequest model);

        /// <summary>
        /// Deletes a taxi by its ID.
        /// </summary>
        /// <param name="taxiId">The ID of the taxi to delete.</param>
        /// <returns>A boolean indicating whether the deletion was successful.</returns>
        Task<bool> DeleteTaxiAsync(int taxiId);
    }
}
