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
        /// Checks whether the currently logged-in user is associated with the given bus company.
        /// </summary>
        Task<bool> ValidateCompanyUser(int companyId);

        /// <summary>
        /// Determines if a given route already exists.
        /// </summary>
        Task<bool> CheckIfRouteExists(AddRouteViewModel model);

        /// <summary>
        /// Adds a new route for the specified company.
        /// </summary>
        Task AddRoute(AddRouteViewModel model, int hours, int minutes, int companyId);

        /// <summary>
        /// Retrieves all routes for the specified company.
        /// </summary>
        Task<List<AddRouteViewModel>> GetRoutes(int companyId);

        /// <summary>
        /// Edits the details of an existing route for the specified company.
        /// </summary>
        Task EditRoute(int routeId, AddRouteViewModel model, int hours, int minutes, int companyId);

        /// <summary>
        /// Deletes an existing route for the specified company.
        /// </summary>
        Task DeleteRoute(int routeId, int companyId);

        /// <summary>
        /// Adds a new schedule to a specified company.
        /// </summary>
        Task AddSchedule(AddScheduleViewModel model, int companyId);

        /// <summary>
        /// Retrieves all schedules associated with the specified company.
        /// </summary>
        Task<List<object>> GetSchedules(int companyId);

        /// <summary>
        /// Edits the details of an existing schedule for the specified company.
        /// </summary>
        Task EditSchedule(int scheduleId, AddScheduleViewModel model, int companyId);

        /// <summary>
        /// Deletes an existing schedule for the specified company.
        /// </summary>
        Task DeleteSchedule(int scheduleId, int companyId);
    }
}