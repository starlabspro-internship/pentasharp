using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pentasharp.Data;
using pentasharp.Interfaces;
using pentasharp.Models.Entities;
using pentasharp.Models.Enums;
using pentasharp.Models.TaxiRequest;
using WebApplication1.Filters;

namespace pentasharp.Controllers
{
    [Route("api/TaxiReservation")]
    public class TaxiReservationController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ITaxiReservationService _taxiReservationService;
        public TaxiReservationController(AppDbContext context, ITaxiReservationService taxiReservationService)
        {
            _context = context;
            _taxiReservationService = taxiReservationService;
        }

        [HttpPost("SearchAvailableTaxis")]
        public async Task<IActionResult> SearchAvailableTaxis()
        {
            var taxiCompanies = await _taxiReservationService.SearchAvailableTaxisAsync();

            return Ok(new { success = true, companies = taxiCompanies });
        }

        [ServiceFilter(typeof(LoginRequiredFilter))]
        [HttpPost("CreateReservation")]
        public async Task<IActionResult> CreateReservation([FromBody] TaxiReservationViewModel model)
        {
            return await _taxiReservationService.CreateReservationAsync(model);
        }

        [ServiceFilter(typeof(AdminOnlyFilter))]
        [HttpGet("GetReservations")]
        public async Task<IActionResult> GetReservations()
        {
            try
            {
                var reservations = await _taxiReservationService.GetReservationsAsync();
                return Ok(new { success = true, reservations });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An internal server error occurred.", error = ex.Message });
            }
        }

        [ServiceFilter(typeof(AdminOnlyFilter))]
        [HttpGet("GetTaxisByTaxiCompany")]
        public async Task<IActionResult> GetTaxisByTaxiCompany(int taxiCompanyId)
        {
            try
            {
                var taxis = await _taxiReservationService.GetTaxisByTaxiCompanyAsync(taxiCompanyId);
                return Ok(taxis);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An internal server error occurred.", error = ex.Message });
            }
        }

        [ServiceFilter(typeof(AdminOnlyFilter))]
        [HttpPut("UpdateReservation/{reservationId}")]
        public async Task<IActionResult> UpdateReservation(int reservationId, [FromBody] UpdateTaxiReservationViewModel model)
        {
            try
            {
                var updated = await _taxiReservationService.UpdateReservationAsync(reservationId, model);

                if (!updated)
                    return NotFound(new { success = false, message = "Reservation or Taxi not found." });

                return Ok(new { success = true, message = "Reservation updated successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An internal server error occurred.", error = ex.Message });
            }
        }
    }
}