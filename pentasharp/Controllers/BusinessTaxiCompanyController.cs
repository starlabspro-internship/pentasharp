using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pentasharp.Models.Enums;
using WebApplication1.Filters;
using pentasharp.Data;
using pentasharp.Services;
using pentasharp.Models.TaxiRequest;

namespace WebApplication1.Controllers
{
    [Route("Business/TaxiCompany")]
    public class BusinessTaxiCompanyController : Controller
    {
        private readonly ITaxiService _taxiService;
        private readonly IDriverService _driverService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;

        public BusinessTaxiCompanyController(
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

        [HttpGet("GetCompany")]
        public IActionResult GetCompany()
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

            if (user.BusinessType == BusinessType.TaxiCompany)
            {
                var company = _context.TaxiCompanies
                    .FirstOrDefault(c => c.UserId == userId.Value);

                if (company == null)
                {
                    return NotFound("No associated taxi company found for this user.");
                }

                return Ok(company);
            }
            else
            {
                return Ok("User is not associated with Taxi Company.");
            }
        }

        [HttpPost("AddTaxi")]
        public async Task<IActionResult> AddTaxi([FromBody] AddTaxiRequest model)
        {

            var userId = GetCompanyId();
            if (!userId.HasValue)
            {
                return Unauthorized(new { success = false, message = "No user is logged in." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, code = ResponseCodes.InvalidData, message = "Invalid data provided." });
            }

            bool isDriverAssigned = await _context.Taxis.AnyAsync(t => t.DriverId == model.DriverId && model.DriverId != null && !t.IsDeleted);
            if (isDriverAssigned)
            {
                return Conflict(new { success = false, message = "Driver is already assigned to another taxi." });
            }

            var taxi = await _taxiService.AddTaxiAsync(model);
            return Ok(new { success = true, code = ResponseCodes.Success, message = "Taxi added successfully." });
        }

        [HttpGet("GetTaxis")]
        public async Task<IActionResult> GetTaxis()
        {
            try
            {
                var userId = GetCompanyId();
                if (!userId.HasValue)
                {
                    return Unauthorized(new { success = false, message = "No user is logged in." });
                }

                var user = await _context.Users.FindAsync(userId.Value);
                if (user == null)
                {
                    return NotFound(new { success = false, message = "User not found." });
                }

                if (!user.CompanyId.HasValue)
                {
                    return NotFound(new { success = false, message = "The logged-in user has no associated company." });
                }

                var companyId = user.CompanyId.Value;
                var taxis = await _taxiService.GetTaxisAsync(companyId);

                return Ok(taxis);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An unexpected error occurred." });
            }
        }

        [HttpPut("EditTaxi/{id}")]
        public async Task<IActionResult> EditTaxi(int id, [FromBody] EditTaxiRequest model)
        {
            var success = await _taxiService.EditTaxiAsync(id, model);
            if (!success)
            {
                return NotFound(new { success = false, message = "Taxi not found." });
            }

            return Ok(new { success = true, message = "Taxi updated successfully." });
        }

        [HttpDelete("DeleteTaxi/{id}")]
        public async Task<IActionResult> DeleteTaxi(int id)
        {
            var success = await _taxiService.DeleteTaxiAsync(id);
            if (!success)
            {
                return NotFound(new { success = false, message = "Taxi not found." });
            }

            return Ok(new { success = true, message = "Taxi deleted successfully." });
        } 

        private int? GetCompanyId()
        {
            return _httpContextAccessor.HttpContext?.Session?.GetInt32("UserId");
        }
    }
}