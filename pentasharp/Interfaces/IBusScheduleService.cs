using pentasharp.ViewModel.BusSchedul;
using pentasharp.ViewModel.BusReservation;

namespace pentasharp.Interfaces
{
    /// <summary>
    /// Provides functionality for managing bus routes, schedules, and related operations.
    /// </summary>
    public interface IBusScheduleService
    {

        /// <summary>
        /// Determines if a given route already exists.
        /// </summary>
        Task<bool> CheckIfRouteExists(AddRouteViewModel model);

        /// <summary>
        /// Adds a new route for the specified company.
        /// </summary>
        Task<bool> AddRoute(AddRouteViewModel model, int hours, int minutes);

        /// <summary>
        /// Retrieves all routes for the specified company.
        /// </summary>
        Task<List<AddRouteViewModel>> GetRoutes();

        /// <summary>
        /// Edits the details of an existing route for the specified company.
        /// </summary>
        Task<bool> EditRoute(int routeId, AddRouteViewModel model, int hours, int minutes);

        /// <summary>
        /// Deletes an existing route for the specified company.
        /// </summary>
        Task<bool> DeleteRoute(int routeId);

        /// <summary>
        /// Adds a new schedule to a specified company.
        /// </summary>
        Task<bool> AddSchedule(AddScheduleViewModel model);

        /// <summary>
        /// Retrieves all schedules associated with the specified company.
        /// </summary>
        Task<List<object>> GetSchedules();

        /// <summary>
        /// Edits the details of an existing schedule for the specified company.
        /// </summary>
        Task<bool> EditSchedule(int scheduleId, AddScheduleViewModel model);

        /// <summary>
        /// Deletes an existing schedule for the specified company.
        /// </summary>
        Task<bool> DeleteSchedule(int scheduleId);
    }
}