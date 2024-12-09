using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using pentasharp.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using pentasharp.Models.Entities;
using AutoMapper;

namespace pentasharp.Controllers
{
    [Route("UserActivity")]
    public class UserActivityController : Controller
    {
        private readonly ITaxiReservationService _taxiReservationService;
        private readonly ILogger<UserActivityController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserActivityController(
            ITaxiReservationService taxiReservationService,
            ILogger<UserActivityController> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _taxiReservationService = taxiReservationService;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("MyReservations")]
        public async Task<IActionResult> MyReservations()
        {

            var userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserId");

            try
            {
                var reservations = await _taxiReservationService.GetReservationsForUserAsync(userId.Value);
                return View("MyReservations", reservations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving reservations for user {UserId}", userId);
                return View("Error", new { message = "An error occurred while retrieving your reservations." });
            }
        }
    }
}
