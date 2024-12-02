using Microsoft.AspNetCore.Mvc;
using pentasharp.Models.DTOs;
using pentasharp.ViewModel.TaxiModels;
using pentasharp.ViewModel.TaxiReservation;

namespace pentasharp.Services
{
    public interface ITaxiReservationService
    {
        Task<IEnumerable<TaxiCompanyDto>> SearchAvailableTaxisAsync();

        Task<IActionResult> CreateReservationAsync(TaxiReservationViewModel model);

        Task<List<TaxiReservationDto>> GetReservationsAsync();

        Task<List<TaxiDto>> GetTaxisByTaxiCompanyAsync(int taxiCompanyId);

        Task<bool> UpdateReservationAsync(int reservationId, UpdateReservationViewModel model);
    }
}