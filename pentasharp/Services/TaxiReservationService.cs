using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using pentasharp.Data;
using pentasharp.Interfaces;
using pentasharp.Models.Entities;
using pentasharp.Models.Enums;
using pentasharp.Models.TaxiRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pentasharp.Services
{
    public class TaxiReservationService : ITaxiReservationService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<TaxiReservationService> _logger;
        private readonly IAuthenticateService _authService;

        public TaxiReservationService(AppDbContext context, IMapper mapper, ILogger<TaxiReservationService> logger, IAuthenticateService authService)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _authService = authService;
        }

        public async Task<IEnumerable<TaxiCompanyRequest>> SearchAvailableTaxisAsync()
        {
            try
            {
                _logger.LogInformation("Searching for available taxis...");
                var taxiCompanies = await _context.TaxiCompanies
                    .Select(c => new TaxiCompanyRequest
                    {
                        TaxiCompanyId = c.TaxiCompanyId,
                        CompanyName = c.CompanyName,
                        ContactInfo = c.ContactInfo
                    })
                    .ToListAsync();

                _logger.LogInformation("Found {Count} taxi companies.", taxiCompanies.Count);
                return taxiCompanies;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching for available taxis in {methodName}.", nameof(SearchAvailableTaxisAsync));
                throw;
            }
        }

        public async Task<IActionResult> CreateReservationAsync(TaxiReservationViewModel model)
        {
            try
            {
             

                if (!TimeSpan.TryParse(model.ReservationTime, out var timeSpan))
                {
                    _logger.LogWarning("Invalid ReservationTime format: {ReservationTime}.", model.ReservationTime);
                    return new BadRequestObjectResult(new { success = false, message = "Invalid ReservationTime format. Must be HH:mm:ss." });
                }

                var reservation = _mapper.Map<TaxiReservations>(model);
                reservation.ReservationTime = model.ReservationDate.Add(timeSpan);
                reservation.Status = ReservationStatus.Pending;
                reservation.CreatedAt = DateTime.UtcNow;
                reservation.Fare = 0;
                _context.TaxiReservations.Add(reservation);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Reservation created successfully with ID {ReservationId}.", reservation.ReservationId);
                return new OkObjectResult(new { success = true, message = "Reservation created successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a reservation in {methodName}.", nameof(CreateReservationAsync));
                return new ObjectResult(new { success = false, message = "An internal server error occurred.", error = ex.Message })
                {
                    StatusCode = 500
                };
            }
        }

        public async Task<List<TaxiReservationRequest>> GetReservationsAsync()
        {
            var companyId = _authService.GetCurrentCompanyId();

            if (!companyId.HasValue)
            {
                throw new UnauthorizedAccessException("No user is logged in or no associated company found.");
            }

            try
            {
                var reservations = await _context.TaxiReservations
                    .Include(r => r.User)
                    .Include(r => r.Taxi)
                    .ThenInclude(t => t.Driver)
                    .Include(r => r.TaxiCompany)
                    .Where(r => r.TaxiCompanyId == companyId.Value)
                    .ToListAsync();

                var reservationInfo = _mapper.Map<List<TaxiReservationRequest>>(reservations);

                foreach (var reservationDto in reservationInfo)
                {
                    var reservationEntity = reservations.FirstOrDefault(r => r.ReservationId == reservationDto.ReservationId);
                    reservationDto.PassengerName = $"{reservationEntity?.User?.FirstName} {reservationEntity?.User?.LastName}".Trim() ?? "Unknown";
                    reservationDto.Driver = reservationEntity?.Taxi != null
                        ? $"{reservationEntity.Taxi.DriverId} - {reservationEntity.Taxi.LicensePlate}"
                        : "Unassigned";
                }

                _logger.LogInformation("Successfully fetched {Count} reservations for company ID: {CompanyId}", reservationInfo.Count, companyId);
                return reservationInfo;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching reservations in {MethodName}.", nameof(GetReservationsAsync));
                throw;
            }
        }

        public async Task<List<TaxiRequest>> GetTaxisByTaxiCompanyAsync(int taxiCompanyId)
        {
            try
            {
                _logger.LogInformation("Fetching taxis for taxi company ID {TaxiCompanyId}.", taxiCompanyId);
                var taxis = await _context.Taxis
                    .Include(t => t.Driver)
                    .Where(t => t.TaxiCompanyId == taxiCompanyId)
                    .ToListAsync();

                _logger.LogInformation("Found {Count} taxis for taxi company ID {TaxiCompanyId}.", taxis.Count, taxiCompanyId);
                return _mapper.Map<List<TaxiRequest>>(taxis);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching taxis for taxi company ID {TaxiCompanyId} in {methodName}.", taxiCompanyId, nameof(GetTaxisByTaxiCompanyAsync));
                throw;
            }
        }

        public async Task<bool> UpdateReservationAsync(int reservationId, UpdateTaxiReservationViewModel model)
        {
            try
            {
                _logger.LogInformation("Updating reservation with ID {ReservationId}.", reservationId);
                var reservation = await _context.TaxiReservations
                    .Include(r => r.Taxi)
                    .FirstOrDefaultAsync(r => r.ReservationId == reservationId);

                if (reservation == null)
                {
                    _logger.LogWarning("Reservation with ID {ReservationId} not found.", reservationId);
                    return false;
                }

                var taxi = model.TaxiId.HasValue && model.TaxiId != 0
                    ? await _context.Taxis.FirstOrDefaultAsync(t => t.TaxiId == model.TaxiId)
                    : null;

                if (taxi == null && model.TaxiId.HasValue && model.TaxiId != 0)
                {
                    _logger.LogWarning("Taxi with ID {TaxiId} not found.", model.TaxiId);
                    return false;
                }

                if (model.ReservationDate.HasValue && !string.IsNullOrEmpty(model.ReservationTime))
                {
                    var reservationDateTime = model.ReservationDate.Value.Add(TimeSpan.Parse(model.ReservationTime));
                    reservation.ReservationTime = reservationDateTime;
                }
                else
                {
                    _logger.LogError("ReservationDate or ReservationTime is missing for reservation ID {ReservationId}.", reservationId);
                    throw new ArgumentException("ReservationDate or ReservationTime is missing.");
                }

                _mapper.Map(model, reservation);
                reservation.UpdatedAt = DateTime.UtcNow;

                _context.TaxiReservations.Update(reservation);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Reservation with ID {ReservationId} updated successfully.", reservationId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating reservation with ID {ReservationId} in {methodName}.", reservationId, nameof(UpdateReservationAsync));
                throw;
            }
        }

        public async Task<List<TaxiReservationRequest>> GetReservationsForUserAsync(int userId)
        {
            try
            {
                var userExists = await _context.Users.AnyAsync(u => u.UserId == userId);
                if (!userExists)
                {
                    _logger.LogWarning("User with ID {UserId} not found", userId);
                    throw new KeyNotFoundException($"User with ID {userId} not found.");
                }

                var reservations = await _context.TaxiReservations
                     .Include(r => r.User)
                     .Include(r => r.Taxi)
                     .ThenInclude(t => t.Driver)
                     .Include(r => r.TaxiCompany)
                     .Where(r => r.UserId == userId)
                     .ToListAsync();

                var reservationInfo = _mapper.Map<List<TaxiReservationRequest>>(reservations);
                return reservationInfo;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching reservations for user {UserId}", userId);
                throw;
            }
        }

        public async Task<bool> CancelReservationAsync(int reservationId, int userId)
        {
            try
            {
                var userExists = await _context.Users.AnyAsync(u => u.UserId == userId);
                if (!userExists)
                {
                    _logger.LogWarning("User with ID {UserId} not found.", userId);
                    return false; 
                }
                var reservation = await _context.TaxiReservations
                    .FirstOrDefaultAsync(r => r.ReservationId == reservationId && r.UserId == userId && r.Status == ReservationStatus.Pending);

                if (reservation == null)
                {
                    _logger.LogWarning("Reservation with ID {ReservationId} cannot be canceled because it is not pending or does not belong to user {UserId}.", reservationId, userId);
                    return false;
                }
                reservation.Status = ReservationStatus.Canceled;
                await _context.SaveChangesAsync();

                _logger.LogInformation("Reservation with ID {ReservationId} canceled successfully.", reservationId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while canceling reservation with ID {ReservationId}.", reservationId);
                throw;
            }
        }

    }
}