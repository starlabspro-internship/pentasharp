using pentasharp.ViewModel.Taxi;
using pentasharp.ViewModel.TaxiModels;
using pentasharp.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using pentasharp.Models.TaxiRequest;

namespace pentasharp.Services
{
    /// <summary>
    /// Provides functionality for managing taxis in the application.
    /// </summary>
    public interface ITaxiService
    {
        /// <summary>
        /// Retrieves a list of taxis associated with a specific taxi company.
        /// </summary>
        Task<List<TaxiRequest>> GetTaxisAsync(int companyId);

        /// <summary>
        /// Adds a new taxi to the system.
        /// </summary>
        Task<Taxi> AddTaxiAsync(AddTaxiRequest model);

        /// <summary>
        /// Edits the details of an existing taxi.
        /// </summary>
        Task<bool> EditTaxiAsync(int id, EditTaxiRequest model);

        /// <summary>
        /// Deletes a taxi by marking it as deleted in the system.
        /// </summary>
        Task<bool> DeleteTaxiAsync(int id);

        /// <summary>
        /// Retrieves a list of available drivers for a specific taxi company and optionally excludes a driver currently assigned to a specific taxi.
        /// </summary>
        Task<List<DriverRequest>> GetAvailableDriversAsync(int companyId, int? taxiId = null);
    }
}