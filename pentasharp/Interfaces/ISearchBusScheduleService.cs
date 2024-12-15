using pentasharp.ViewModel.BusReservation;
using pentasharp.ViewModel.BusSchedul;

namespace pentasharp.Interfaces
{
    public interface ISearchBusScheduleService
    {
        Task<List<SearchScheduleViewModel>> SearchSchedulesAsync(string from, string to, DateTime date);
        Task<string[]> GetFromLocationSuggestionsAsync(string query);
        Task<string[]> GetToLocationSuggestionsAsync(string fromLocation, string query);
        Task<int> AddReservationAsync(AddBusReservationViewModel model);
    }
}