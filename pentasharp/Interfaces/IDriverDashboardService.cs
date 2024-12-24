using Microsoft.AspNetCore.Mvc;
using pentasharp.Models.TaxiRequest;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pentasharp.Interfaces
{
    /// <summary>
    /// Interface for driver dashboard-related services.
    /// </summary>
    public interface IDriverDashboardService
    {
        /// <summary>
        /// Retrieves all pending taxi bookings assigned to the current driver.
        /// </summary>
        /// <returns>
        /// A list of <see cref="TaxiBookingViewModel"/> representing the pending bookings.
        /// If no bookings are found or the driver is not authenticated, an empty list is returned.
        /// </returns>
        Task<List<TaxiBookingViewModel>> GetAllBookingsAsync();

        /// <summary>
        /// Claims a specific booking for the current driver.
        /// </summary>
        /// <param name="bookingId">The ID of the booking to be claimed.</param>
        /// <returns>
        /// A boolean value indicating whether the booking was successfully claimed.
        /// Returns <c>false</c> if the booking does not exist, is not pending, or belongs to another driver.
        /// </returns>
        Task<bool> ClaimBookingAsync(int bookingId);

        /// <summary>
        /// Starts a trip for a specified booking, updating its status to "In Progress" 
        /// and setting the trip start time to the current time.
        /// </summary>
        /// <param name="bookingId">The ID of the booking for which the trip is being started.</param>
        /// <returns>
        /// A tuple containing:
        /// - <c>Success</c>: A boolean indicating if the operation was successful.
        /// - <c>Message</c>: A descriptive message about the operation result.
        /// </returns>
        Task<(bool Success, string Message)> StartTripAsync(int bookingId);

        /// <summary>
        /// Ends a trip for a specified booking, updating its status to "Completed" and setting the trip end time and fare.
        /// </summary>
        /// <param name="bookingId">The ID of the booking for which the trip is being ended.</param>
        /// <param name="fare">The total fare for the completed trip.</param>
        /// <returns>
        /// A tuple containing:
        /// - <c>Success</c>: A boolean indicating if the operation was successful.
        /// - <c>Message</c>: A descriptive message about the operation result.
        /// - <c>Booking</c>: The updated <see cref="TaxiBookingViewModel"/> object for the booking.
        /// </returns>
        Task<(bool Success, string Message, TaxiBookingViewModel Booking)> EndTripAsync(int bookingId, decimal fare);

        /// <summary>
        /// Retrieves a list of taxi reservations associated with the current driver.
        /// </summary>
        /// <returns>
        /// A list of <see cref="TaxiReservationRequest"/> representing the reservations.
        /// If no reservations are found or the driver is not authenticated, an empty list is returned.
        /// </returns>
        Task<List<TaxiReservationRequest>> GetReservationsAsync();

        /// <summary>
        /// Accepts a specific reservation request for the current driver.
        /// </summary>
        /// <param name="reservationId">The ID of the reservation to be accepted.</param>
        /// <returns>
        /// A boolean value indicating whether the reservation was successfully accepted.
        /// Returns <c>false</c> if the reservation does not exist or the driver cannot accept it.
        /// </returns>
        Task<bool> AcceptReservationAsync(int reservationId);

        /// <summary>
        /// Starts a trip for a specific reservation, updating its status to "In Progress" 
        /// and setting the trip start time to the current time.
        /// </summary>
        /// <param name="reservationId">The ID of the reservation for which the trip is being started.</param>
        /// <returns>
        /// A tuple containing:
        /// - <c>Success</c>: A boolean indicating if the operation was successful.
        /// - <c>Message</c>: A descriptive message about the operation result.
        /// </returns>
        Task<(bool Success, string Message)> StartReservationTripAsync(int reservationId);

        /// <summary>
        /// Ends a trip for a specific reservation, updating its status to "Completed" 
        /// and setting the trip end time and fare.
        /// </summary>
        /// <param name="reservationId">The ID of the reservation for which the trip is being ended.</param>
        /// <returns>
        /// A tuple containing:
        /// - <c>Success</c>: A boolean indicating if the operation was successful.
        /// - <c>Message</c>: A descriptive message about the operation result.
        /// </returns>
        Task<(bool Success, string Message)> EndReservationTripAsync(int reservationId);
    }
}
