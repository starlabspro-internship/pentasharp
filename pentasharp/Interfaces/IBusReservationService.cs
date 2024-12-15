using pentasharp.ViewModel.BusReservation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pentasharp.Interfaces
{
    /// <summary>
    /// Provides functionality for managing bus reservations.
    /// </summary>
    public interface IBusReservationService
    {
        /// <summary>
        /// Retrieves all reservations for a specified company.
        /// </summary>
        Task<List<object>> GetReservationsAsync(int companyId);

        /// <summary>
        /// Confirms a pending reservation.
        /// </summary>
        Task<object> ConfirmReservationAsync(EditReservationViewModel model);

        /// <summary>
        /// Cancels an existing reservation.
        /// </summary>
        Task<object> CancelReservationAsync(EditReservationViewModel model);
    }
}