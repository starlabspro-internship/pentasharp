using pentasharp.ViewModel.Bus;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pentasharp.Interfaces
{
    /// <summary>
    /// Provides functionality for managing buses associated with a user.
    /// </summary>
    public interface IBusService
    {
        /// <summary>
        /// Asynchronously retrieves a list of buses associated with a specific user.
        /// </summary>
        Task<List<BusViewModel>> GetBusesAsync();

        /// <summary>
        /// Asynchronously adds a new bus associated with a specific user.
        /// </summary>
        Task<bool> AddBusAsync(AddBusViewModel model);

        /// <summary>
        /// Asynchronously edits the details of an existing bus belonging to a specific user.
        /// </summary>
        Task<bool> EditBusAsync(int id, EditBusViewModel model);

        /// <summary>
        /// Asynchronously deletes an existing bus belonging to a specific user.
        /// </summary>
        Task<bool> DeleteBusAsync(int id);
    }
}