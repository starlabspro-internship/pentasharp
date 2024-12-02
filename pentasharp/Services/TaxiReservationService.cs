using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pentasharp.Data;
using pentasharp.Interfaces;
using pentasharp.Models.DTOs;
using pentasharp.Models.Entities;
using pentasharp.Models.Enums;
using pentasharp.ViewModel.TaxiModels;
using pentasharp.ViewModel.TaxiReservation;

namespace pentasharp.Services
{
    public class TaxiReservationService : ITaxiReservationService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public TaxiReservationService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TaxiCompanyDto>> SearchAvailableTaxisAsync()
        {
            var taxiCompanies = await _context.TaxiCompanies
                .Select(c => new TaxiCompanyDto
                {
                    TaxiCompanyId = c.TaxiCompanyId,
                    CompanyName = c.CompanyName,
                    ContactInfo = c.ContactInfo
                })
                .ToListAsync();

            return taxiCompanies;
        }

        public async Task<IActionResult> CreateReservationAsync(TaxiReservationViewModel model)
        {
            try
            {
                if (!TimeSpan.TryParse(model.ReservationTime, out var timeSpan))
                {
                    return new BadRequestObjectResult(new { success = false, message = "Invalid ReservationTime format. Must be HH:mm:ss." });
                }

                var reservation = _mapper.Map<TaxiReservations>(model);
                reservation.ReservationTime = model.ReservationDate.Add(timeSpan);
                reservation.Status = ReservationStatus.Pending;
                reservation.CreatedAt = DateTime.UtcNow;
                reservation.Fare = 0;
                _context.TaxiReservations.Add(reservation);
                await _context.SaveChangesAsync();

                return new OkObjectResult(new { success = true, message = "Reservation created successfully." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
                return new ObjectResult(new { success = false, message = "An internal server error occurred.", error = ex.Message })
                {
                    StatusCode = 500
                };
            }
        }

        public async Task<List<TaxiReservationDto>> GetReservationsAsync()
        {
            try
            {
                // Fetch reservations with eager loading
                var reservations = await _context.TaxiReservations
                    .Include(r => r.User)
                    .Include(r => r.Taxi)
                    .Include(r => r.TaxiCompany)
                    .ToListAsync();
                var reservationDtos = _mapper.Map<List<TaxiReservationDto>>(reservations);
                foreach (var reservationDto in reservationDtos)
                {
                    var reservationEntity = reservations.FirstOrDefault(r => r.ReservationId == reservationDto.ReservationId);
                    reservationDto.PassengerName = reservationEntity?.User != null ? reservationEntity.User.FirstName : "Unknown";

                    reservationDto.Driver = reservationEntity?.Taxi != null
               ? $"{reservationEntity.Taxi.DriverName} - {reservationEntity.Taxi.LicensePlate}"
               : "Unassigned";
                }

                return reservationDtos;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching reservations.", ex);
            }
        }

        public async Task<List<TaxiDto>> GetTaxisByTaxiCompanyAsync(int taxiCompanyId)
        {
            var taxis = await _context.Taxis
                .Where(t => t.TaxiCompanyId == taxiCompanyId)
                .ToListAsync();

            return _mapper.Map<List<TaxiDto>>(taxis);
        }

        public async Task<bool> UpdateReservationAsync(int reservationId, UpdateReservationViewModel model)
        {
            var reservation = await _context.TaxiReservations
                .Include(r => r.Taxi)
                .FirstOrDefaultAsync(r => r.ReservationId == reservationId);

            if (reservation == null) return false;

            var taxi = model.TaxiId.HasValue && model.TaxiId != 0
                ? await _context.Taxis.FirstOrDefaultAsync(t => t.TaxiId == model.TaxiId)
                : null;

            if (taxi == null && model.TaxiId.HasValue && model.TaxiId != 0) return false;

            if (model.ReservationDate.HasValue && !string.IsNullOrEmpty(model.ReservationTime))
            {
                var reservationDateTime = model.ReservationDate.Value
                    .Add(TimeSpan.Parse(model.ReservationTime));
                reservation.ReservationTime = reservationDateTime;
            }
            else
            {
                throw new ArgumentException("ReservationDate or ReservationTime is missing.");
            }

            // Map other fields
            _mapper.Map(model, reservation);
            reservation.UpdatedAt = DateTime.UtcNow;

            _context.TaxiReservations.Update(reservation);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}