using Microsoft.AspNetCore.Mvc;
using pentasharp.Models.TaxiRequest;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pentasharp.Interfaces
{
    /// <summary>
    /// Defines the contract for managing taxi reservations.
    /// </summary>
    public interface ITaxiReservationService
    {
        /// <summary>
        /// Searches for available taxi companies.
        /// </summary>
        /// <returns>The model representing a collection of available taxi companies.</returns>
        Task<IEnumerable<TaxiCompanyRequest>> SearchAvailableTaxisAsync();

        /// <summary>
        /// Creates a new taxi reservation.
        /// </summary>
        /// <param name="model">The model containing reservation details.</param>
        /// <returns>The model representing an IActionResult indicating the result of the operation.</returns>
        Task<IActionResult> CreateReservationAsync(TaxiReservationViewModel model);

        /// <summary>
        /// Retrieves a list of all taxi reservations.
        /// </summary>
        /// <returns>The model representing a list of taxi reservations.</returns>
        Task<List<TaxiReservationRequest>> GetReservationsAsync();

        /// <summary>
        /// Retrieves a list of taxis for a specific taxi company.
        /// </summary>
        /// <param name="taxiCompanyId">The ID of the taxi company.</param>
        /// <returns>The model representing a list of taxis.</returns>
        Task<List<TaxiRequest>> GetTaxisByTaxiCompanyAsync(int taxiCompanyId);

        /// <summary>
        /// Updates an existing taxi reservation.
        /// </summary>
        /// <param name="reservationId">The ID of the reservation to update.</param>
        /// <param name="model">The model containing updated reservation details.</param>
        /// <returns>The model representing a boolean indicating whether the update was successful.</returns>
        Task<bool> UpdateReservationAsync(int reservationId, UpdateTaxiReservationViewModel model);
    }
}