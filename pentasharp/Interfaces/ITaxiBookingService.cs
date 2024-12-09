using pentasharp.Models.DTOs;
using pentasharp.Models.TaxiRequest;
using pentasharp.ViewModel.TaxiModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pentasharp.Interfaces
{
    /// <summary>
    /// Interface for managing taxi bookings.
    /// </summary>
    public interface ITaxiBookingService
    {
        /// <summary>
        /// Retrieves all taxi companies.
        /// </summary>
        /// <returns>A list of <see cref="TaxiCompanyViewModel"/> representing all taxi companies.</returns>
        Task<List<TaxiCompanyViewModel>> GetAllCompaniesAsync();

        /// <summary>
        /// Creates a new taxi booking.
        /// </summary>
        /// <param name="model">The <see cref="CreateBookingViewModel"/> containing booking details.</param>
        /// <returns>A boolean indicating whether the booking was created successfully.</returns>
        Task<bool> CreateBookingAsync(TaxiBookingRequest request);

        /// <summary>
        /// Retrieves all taxi bookings.
        /// </summary>
        /// <returns>A list of <see cref="TaxiBookingViewModel"/> representing all bookings.</returns>
        Task<List<TaxiBookingViewModel>> GetAllBookingsAsync(int userId);

        /// <summary>
        /// Retrieves a taxi booking by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the booking.</param>
        /// <returns>A <see cref="TaxiBookingViewModel"/> representing the booking, or null if not found.</returns>
        Task<TaxiBookingViewModel> GetBookingByIdAsync(int id);

        /// <summary>
        /// Updates an existing taxi booking.
        /// </summary>
        /// <param name="model">The <see cref="EditTaxiBookingViewModel"/> containing updated booking details.</param>
        /// <returns>A boolean indicating whether the booking was updated successfully.</returns>
        Task<bool> UpdateBookingAsync(EditTaxiBookingViewModel model);
    }
}