using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pentasharp.Data;
using pentasharp.Interfaces;
using pentasharp.Models.Entities;
using pentasharp.Models.Enums;
using pentasharp.ViewModel.Bus;
using WebApplication1.Filters;

namespace WebApplication1.Controllers
{
    [Route("Business/BusCompany")]
    [TypeFilter(typeof(BusinessOnlyFilter), Arguments = new object[] { "BusCompany" })]
    public class BusinessBusCompanyController : Controller
    {
        private readonly IBusCompanyService _companyService;
        private readonly IBusService _busService;
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public BusinessBusCompanyController( IBusService busService, IHttpContextAccessor httpContextAccessor, AppDbContext context, IMapper mapper, IBusCompanyService companyService)
        {
            _busService = busService;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mapper = mapper;
            _companyService = companyService;
        }

        [HttpGet("GetCompany")]
        public async Task<IActionResult> GetCompanyAsync()
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized(new { success = false, message = "User not logged in." });

            var company = await _companyService.GetCompanyByUserIdAsync(userId.Value);
            if (company != null)
                return Ok(company);

            return NotFound(new { success = false, message = "No company associated with the logged-in user." });
        }

        [HttpPost("AddBus")]
        public async Task<IActionResult> AddBusAsync([FromBody] AddBusViewModel model)
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized(new { success = false, message = "User not logged in." });

            var result = await _busService.AddBusAsync(model, userId.Value);
            if (result)
                return Ok(new { success = true, message = "Bus added successfully." });

            return BadRequest(new { success = false, message = "Invalid data provided." });
        }

        [HttpGet("GetBuses")]
        public async Task<IActionResult> GetBusesAsync()
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized(new { success = false, message = "User not logged in." });

            var buses = await _busService.GetBusesAsync(userId.Value);
            if (buses != null)
                return Ok(buses);

            return NotFound(new { success = false, message = "No buses found." });
        }

        [HttpPut("EditBus/{id}")]
        public async Task<IActionResult> EditBusAsync(int id, [FromBody] EditBusViewModel model)
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized(new { success = false, message = "User not logged in." });

            var result = await _busService.EditBusAsync(id, model, userId.Value);
            if (result)
                return Ok(new { success = true, message = "Bus updated successfully." });

            return NotFound(new { success = false, message = "Bus not found." });
        }


        [HttpDelete("DeleteBus/{id}")]
        public async Task<IActionResult> DeleteBusAsync(int id)
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized(new { success = false, message = "User not logged in." });

            var result = await _busService.DeleteBusAsync(id, userId.Value);
            if (result)
                return Ok(new { success = true, message = "Bus deleted successfully." });

            return NotFound(new { success = false, message = "Bus not found." });
        }

        private int? GetUserId()
        {
            return _httpContextAccessor.HttpContext?.Session?.GetInt32("UserId");
        }
    }
}