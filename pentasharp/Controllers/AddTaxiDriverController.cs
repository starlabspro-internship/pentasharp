using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pentasharp.Models.Enums;
using WebApplication1.Filters;
using pentasharp.Data;
using pentasharp.Services;
using pentasharp.Models.TaxiRequest;

namespace WebApplication1.Controllers
{
    [Route("Business/TaxiDriver")]
    public class AddTaxiDriverController : Controller
    {
        private readonly ITaxiService _taxiService;
        private readonly IDriverService _driverService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;

        public AddTaxiDriverController(
            ITaxiService taxiService,
            IDriverService driverService,
            IHttpContextAccessor httpContextAccessor,
            AppDbContext context)
        {
            _taxiService = taxiService;
            _driverService = driverService;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        [HttpPost("AddDriver")]
        public async Task<IActionResult> AddDriver([FromBody] RegisterDriverRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Invalid data provided." });
            }

            var userId = GetCompanyId();
            if (!userId.HasValue)
            {
                return Unauthorized("No user is logged in.");
            }

            var user = await _context.Users.FindAsync(userId.Value);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var company = await _context.TaxiCompanies.FirstOrDefaultAsync(c => c.UserId == user.UserId);
            if (company == null)
            {
                return NotFound("No associated taxi company found for this user.");
            }

            var newDriver = await _driverService.AddDriverAsync(model, company.TaxiCompanyId);
            return Ok(new { success = true, message = "Driver added successfully." });
        }

        [HttpGet("GetDrivers")]
        public IActionResult GetDrivers()
        {
            var userId = GetCompanyId();
            if (!userId.HasValue)
            {
                return Unauthorized("No user is logged in.");
            }

            var user = _context.Users
                  .Where(u => u.UserId == userId.Value && u.BusinessType == BusinessType.TaxiCompany)
                  .FirstOrDefault();

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var company = _context.TaxiCompanies.FirstOrDefault(c => c.UserId == user.UserId);
            if (company == null)
            {
                return NotFound("No associated taxi company found for this user.");
            }

            var drivers = _driverService.GetDrivers(company.TaxiCompanyId);
            return Ok(drivers);
        }

        [HttpGet("GetAvailableDrivers/{taxiId?}")]
        public async Task<IActionResult> GetAvailableDrivers(int? taxiId = null)
        {
            var userId = GetCompanyId();
            if (!userId.HasValue)
            {
                return Unauthorized(new { success = false, code = ApiStatusEnum.UNAUTHORIZED, message = "No user is logged in." });
            }

            var user = await _context.Users.FindAsync(userId.Value);
            if (user == null)
            {
                return NotFound(new { success = false, code = ResponseCodes.NotFound, message = "User not found." });
            }

            var company = await _context.TaxiCompanies.FirstOrDefaultAsync(c => c.UserId == user.UserId);
            if (company == null)
            {
                return NotFound(new { success = false, code = ResponseCodes.NotFound, message = "No associated taxi company found for this user." });
            }

            var drivers = await _taxiService.GetAvailableDriversAsync(company.TaxiCompanyId, taxiId);
            return Ok(new { success = true, drivers });
        }

        [HttpPut("EditDriver/{id}")]
        public async Task<IActionResult> EditDriver(int id, [FromBody] EditDriverRequest model)
        {
            var success = await _driverService.EditDriverAsync(id, model);
            if (!success)
            {
                return NotFound(new { success = false, message = "Driver not found." });
            }

            return Ok(new { success = true, message = "Driver updated successfully." });
        }

        [HttpDelete("DeleteDriver/{id}")]
        public async Task<IActionResult> DeleteDriver(int id)
        {
            var success = await _driverService.DeleteDriverAsync(id);
            if (!success)
            {
                return NotFound(new { success = false, message = "Driver not found." });
            }

            return Ok(new { success = true, message = "Driver deleted successfully." });
        }

        private int? GetCompanyId()
        {
            return _httpContextAccessor.HttpContext?.Session?.GetInt32("UserId");
        }
    }
}