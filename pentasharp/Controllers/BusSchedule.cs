using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using pentasharp.Interfaces;
using pentasharp.ViewModel.BusSchedul;
using pentasharp.ViewModel.BusReservation;
using WebApplication1.Filters;
using pentasharp.Models.Enums;
using pentasharp.Models.Utilities;

namespace WebApplication1.Controllers
{
    [Route("api/BusSchedule")]
    [TypeFilter(typeof(BusinessOnlyFilter), Arguments = new object[] { "BusCompany" })]
    public class BusScheduleController : Controller
    {
        private readonly IBusScheduleService _busScheduleService;

        public BusScheduleController(IBusScheduleService busScheduleService)
        {
            _busScheduleService = busScheduleService;
        }

        public IActionResult BusScheduleManagement() => View();

        [Route("BusReservationManagement")]
        public IActionResult BusReservationManagement() => View();

        [HttpPost("AddRoute")]
        public async Task<IActionResult> AddRoute([FromBody] AddRouteViewModel model, int hours, int minutes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ResponseFactory.ErrorResponse(ResponseCodes.InvalidData, "Invalid route data provided."));
            }

            try
            {
                var isAdded = await _busScheduleService.AddRoute(model, hours, minutes);

                if (!isAdded)
                {
                    return StatusCode(500, ResponseFactory.ErrorResponse(ResponseCodes.InternalServerError, "Failed to add the route."));
                }

                return Ok(ResponseFactory.SuccessResponse("Route added successfully.", model));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseFactory.ErrorResponse(ResponseCodes.InternalServerError, "An error occurred while adding the route."));
            }
        }


        [HttpGet("GetRoutes")]
        public async Task<IActionResult> GetRoutes()
        {
            try
            {
                var routes = await _busScheduleService.GetRoutes();

                if (routes == null || !routes.Any())
                {
                    return NotFound(ResponseFactory.ErrorResponse(ResponseCodes.NotFound, "No routes found."));
                }

                return Ok(routes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseFactory.ErrorResponse(ResponseCodes.InternalServerError, "An error occurred while retrieving routes."));
            }
        }

        [HttpPut("EditRoute/{id}")]
        public async Task<IActionResult> EditRoute(int id, [FromBody] AddRouteViewModel model, int hours, int minutes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ResponseFactory.ErrorResponse(ResponseCodes.InvalidData, "Invalid route data provided."));
            }

            try
            {
                var isUpdated = await _busScheduleService.EditRoute(id, model, hours, minutes);

                if (!isUpdated)
                {
                    return NotFound(ResponseFactory.ErrorResponse(ResponseCodes.NotFound, $"Route with ID {id} not found or update failed."));
                }

                return Ok(ResponseFactory.SuccessResponse("Route updated successfully.", model));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseFactory.ErrorResponse(ResponseCodes.InternalServerError, "An error occurred while updating the route."));
            }
        }

        [HttpDelete("DeleteRoute/{id}")]
        public async Task<IActionResult> DeleteRoute(int id)
        {
            try
            {
                var isDeleted = await _busScheduleService.DeleteRoute(id);

                if (!isDeleted)
                {
                    return NotFound(ResponseFactory.ErrorResponse(ResponseCodes.NotFound, $"Route with ID {id} not found or could not be deleted."));
                }

                return Ok(ResponseFactory.SuccessResponse("Route deleted successfully.", id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseFactory.ErrorResponse(ResponseCodes.InternalServerError, "An error occurred while deleting the route."));
            }
        }


        [HttpPost("AddSchedule")]
        public async Task<IActionResult> AddSchedule([FromBody] AddScheduleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ResponseFactory.ErrorResponse(ResponseCodes.InvalidData, "Invalid schedule data provided."));
            }

            try
            {
                var result = await _busScheduleService.AddSchedule(model);

                if (!result)
                {
                    return StatusCode(500, ResponseFactory.ErrorResponse(ResponseCodes.InternalServerError, "Failed to add schedule."));
                }

                return Ok(ResponseFactory.SuccessResponse("Schedule added successfully.", model));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseFactory.ErrorResponse(ResponseCodes.InternalServerError, "An error occurred while adding the schedule."));
            }
        }

        [HttpGet("GetSchedules")]
        public async Task<IActionResult> GetSchedules()
        {
            try
            {
                var schedules = await _busScheduleService.GetSchedules();

                if (schedules == null || !schedules.Any())
                {
                    return NotFound(ResponseFactory.ErrorResponse(ResponseCodes.NotFound, "No schedules found."));
                }

                return Ok(schedules);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseFactory.ErrorResponse(ResponseCodes.InternalServerError, "An error occurred while retrieving schedules."));
            }
        }


        [HttpPut("EditSchedule/{id}")]
        public async Task<IActionResult> EditSchedule(int id, [FromBody] AddScheduleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ResponseFactory.ErrorResponse(ResponseCodes.InvalidData, "Invalid schedule data provided."));
            }

            var isUpdated = await _busScheduleService.EditSchedule(id, model);

            if (!isUpdated)
            {
                return NotFound(ResponseFactory.ErrorResponse(ResponseCodes.NotFound, $"Schedule with ID {id} not found or update failed."));
            }

            return Ok(ResponseFactory.SuccessResponse("Schedule updated successfully.", model));
        }

        [HttpDelete("DeleteSchedule/{id}")]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            var isDeleted = await _busScheduleService.DeleteSchedule(id);

            if (!isDeleted)
            {
                return NotFound(ResponseFactory.ErrorResponse(ResponseCodes.NotFound, "Schedule not found or already deleted."));
            }

            return Ok(ResponseFactory.SuccessResponse("Schedule deleted successfully.", id));
        }
    }
}