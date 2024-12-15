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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BusScheduleController(IBusScheduleService busScheduleService, IHttpContextAccessor httpContextAccessor)
        {
            _busScheduleService = busScheduleService;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult BusScheduleManagement() => View();

        [Route("BusReservationManagement")]
        public IActionResult BusReservationManagement() => View();

        [HttpPost("AddRoute")]
        public async Task<IActionResult> AddRoute([FromBody] AddRouteViewModel model, int hours, int minutes)
        {
            var companyId = GetCompanyId();
            if (!companyId.HasValue)
                return Unauthorized(ResponseFactory.UnauthorizedResponse());

            if (!await _busScheduleService.ValidateCompanyUser(companyId.Value))
                return Unauthorized(ResponseFactory.ForbiddenResponse());

            await _busScheduleService.AddRoute(model, hours, minutes, companyId.Value);

            return Ok(ResponseFactory.SuccessResponse("Route added successfully.", model));
        }

        [HttpGet("GetRoutes")]
        public async Task<IActionResult> GetRoutes()
        {
            var companyId = GetCompanyId();
            if (!companyId.HasValue)
                return Unauthorized(ResponseFactory.UnauthorizedResponse());

            if (!await _busScheduleService.ValidateCompanyUser(companyId.Value))
                return Unauthorized(ResponseFactory.ForbiddenResponse());

            var routes = await _busScheduleService.GetRoutes(companyId.Value);
            return Ok(routes);
        }

        [HttpPut("EditRoute/{id}")]
        public async Task<IActionResult> EditRoute(int id, [FromBody] AddRouteViewModel model, int hours, int minutes)
        {
            var companyId = GetCompanyId();
            if (!companyId.HasValue)
                return Unauthorized(ResponseFactory.UnauthorizedResponse());

            if (!await _busScheduleService.ValidateCompanyUser(companyId.Value))
                return Unauthorized(ResponseFactory.ForbiddenResponse());

            await _busScheduleService.EditRoute(id, model, hours, minutes, companyId.Value);

            return Ok(ResponseFactory.SuccessResponse("Route updated successfully.", model));
        }

        [HttpDelete("DeleteRoute/{id}")]
        public async Task<IActionResult> DeleteRoute(int id)
        {
            var companyId = GetCompanyId();
            if (!companyId.HasValue)
                return Unauthorized(ResponseFactory.UnauthorizedResponse());

            if (!await _busScheduleService.ValidateCompanyUser(companyId.Value))
                return Unauthorized(ResponseFactory.ForbiddenResponse());

            await _busScheduleService.DeleteRoute(id, companyId.Value);

            return Ok(ResponseFactory.SuccessResponse("Route deleted successfully.",id));
        }

        [HttpPost("AddSchedule")]
        public async Task<IActionResult> AddSchedule([FromBody] AddScheduleViewModel model)
        {
            var companyId = GetCompanyId();
            if (!companyId.HasValue)
                return Unauthorized(ResponseFactory.UnauthorizedResponse());

            if (!await _busScheduleService.ValidateCompanyUser(companyId.Value))
                return Unauthorized(ResponseFactory.ForbiddenResponse());

            await _busScheduleService.AddSchedule(model, companyId.Value);

            return Ok(ResponseFactory.SuccessResponse("Schedule added successfully.",model));
        }

        [HttpGet("GetSchedules")]
        public async Task<IActionResult> GetSchedules()
        {
            var companyId = GetCompanyId();
            if (!companyId.HasValue)
                return Unauthorized(ResponseFactory.UnauthorizedResponse());

            if (!await _busScheduleService.ValidateCompanyUser(companyId.Value))
                return Unauthorized(ResponseFactory.ForbiddenResponse());

            var schedules = await _busScheduleService.GetSchedules(companyId.Value);
            return Ok(schedules);
        }

        [HttpPut("EditSchedule/{id}")]
        public async Task<IActionResult> EditSchedule(int id, [FromBody] AddScheduleViewModel model)
        {
            var companyId = GetCompanyId();
            if (!companyId.HasValue)
                return Unauthorized(ResponseFactory.UnauthorizedResponse());

            if (!await _busScheduleService.ValidateCompanyUser(companyId.Value))
                return Unauthorized(ResponseFactory.ForbiddenResponse());

            await _busScheduleService.EditSchedule(id, model, companyId.Value);

            return Ok(ResponseFactory.SuccessResponse("Schedule updated successfully.", model));
        }

        [HttpDelete("DeleteSchedule/{id}")]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            var companyId = GetCompanyId();
            if (!companyId.HasValue)
                return Unauthorized(ResponseFactory.UnauthorizedResponse());

            if (!await _busScheduleService.ValidateCompanyUser(companyId.Value))
                return Unauthorized(ResponseFactory.ForbiddenResponse());

            await _busScheduleService.DeleteSchedule(id, companyId.Value);

            return Ok(ResponseFactory.SuccessResponse("Schedule deleted successfully.", id));
        }

        private int? GetCompanyId()
        {
            return _httpContextAccessor.HttpContext?.Session?.GetInt32("CompanyId");
        }
    }
}