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
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TaxiReservationController(AppDbContext context, ITaxiReservationService taxiReservationService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _taxiReservationService = taxiReservationService;
            _httpContextAccessor = httpContextAccessor;
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
    }
}