using pentasharp.ViewModel.BusReservation;

namespace pentasharp.Interfaces
{
    public interface IBusReservationService
    {
        Task<List<object>> GetReservationsAsync(int companyId);
        Task<object> ConfirmReservationAsync(EditReservationViewModel model);
        Task<object> CancelReservationAsync(EditReservationViewModel model);
    }
}
