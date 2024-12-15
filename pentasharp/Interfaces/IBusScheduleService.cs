using pentasharp.ViewModel.BusSchedul;
using pentasharp.ViewModel.BusReservation;

namespace pentasharp.Interfaces
{
    public interface IBusScheduleService
    {
        Task<bool> ValidateCompanyUser(int companyId);
        Task<bool> CheckIfRouteExists(AddRouteViewModel model);
        Task AddRoute(AddRouteViewModel model, int hours, int minutes, int companyId);
        Task<List<AddRouteViewModel>> GetRoutes(int companyId);
        Task EditRoute(int routeId, AddRouteViewModel model, int hours, int minutes, int companyId);
        Task DeleteRoute(int routeId, int companyId);
        Task AddSchedule(AddScheduleViewModel model, int companyId);
        Task<List<object>> GetSchedules(int companyId);
        Task EditSchedule(int scheduleId, AddScheduleViewModel model, int companyId);
        Task DeleteSchedule(int scheduleId, int companyId);
    }
}