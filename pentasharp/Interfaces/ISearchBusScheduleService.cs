using pentasharp.ViewModel.BusReservation;
using pentasharp.ViewModel.BusSchedul;

namespace pentasharp.Interfaces
{
    /// <summary>
    /// Defines the contract for operations related to searching bus schedules and managing reservations.
    /// </summary>
    public interface ISearchBusScheduleService
    {
        /// <summary>
        /// Searches for bus schedules based on the provided origin, destination, and date.
        /// </summary>
        Task<List<SearchScheduleViewModel>> SearchSchedulesAsync(string from, string to, DateTime date);

        /// <summary>
        /// Retrieves location suggestions for the origin based on the user's query.
        /// </summary>
        Task<string[]> GetFromLocationSuggestionsAsync(string query);

        /// <summary>
        /// Retrieves location suggestions for the destination based on the user's query and origin location.
        /// </summary>
        Task<string[]> GetToLocationSuggestionsAsync(string fromLocation, string query);

        /// <summary>
        /// Adds a bus reservation based on the provided reservation details.
        /// </summary>
        Task<int> AddReservationAsync(AddBusReservationViewModel model);
    }
}