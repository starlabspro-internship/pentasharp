using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pentasharp.Models.Enums;
using WebApplication1.Filters;
using pentasharp.Data;
using pentasharp.Services;
using pentasharp.Models.TaxiRequest;
using pentasharp.Models.Utilities;

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
                return Unauthorized(ResponseFactory.ErrorResponse(ResponseCodes.Unauthorized, ResponseMessages.Unauthorized));
            }

            var user = _context.Users
                 .Where(u => u.UserId == userId.Value && u.BusinessType == BusinessType.TaxiCompany)
                 .FirstOrDefault();
            if (user == null)
            {
                return NotFound(ResponseFactory.ErrorResponse(ResponseCodes.NotFound, "User not found."));
            }

            if (user.BusinessType == BusinessType.TaxiCompany)
            {
                var company = _context.TaxiCompanies
                    .FirstOrDefault(c => c.UserId == userId.Value);

                if (company == null)
                {
                    return NotFound(ResponseFactory.ErrorResponse(ResponseCodes.NotFound, "No associated taxi company found for this user."));
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
                return Unauthorized(ResponseFactory.ErrorResponse(ResponseCodes.Unauthorized, ResponseMessages.Unauthorized));
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ResponseFactory.ErrorResponse(ResponseCodes.InvalidData, ResponseMessages.InvalidData));
            }

            bool isDriverAssigned = await _context.Taxis.AnyAsync(t => t.DriverId == model.DriverId && model.DriverId != null && !t.IsDeleted);
            if (isDriverAssigned)
            {
                return Conflict(ResponseFactory.ErrorResponse(ResponseCodes.Conflict, "Driver is already assigned to another taxi."));
            }

            var taxi = await _taxiService.AddTaxiAsync(model);
            return Ok(ResponseFactory.SuccessResponse("Taxi added successfully.",taxi));
        }

        [HttpGet("GetTaxis")]
        public async Task<IActionResult> GetTaxis()
        {
            try
            {
                var userId = GetCompanyId();
                if (!userId.HasValue)
                {
                    return Unauthorized(ResponseFactory.ErrorResponse(ResponseCodes.Unauthorized, ResponseMessages.Unauthorized));
                }

                var user = await _context.Users.FindAsync(userId.Value);
                if (user == null)
                {
                    return NotFound(ResponseFactory.ErrorResponse(ResponseCodes.NotFound, "User not found."));
                }

                if (!user.CompanyId.HasValue)
                {
                    return NotFound(ResponseFactory.ErrorResponse(ResponseCodes.NotFound, "The logged-in user has no associated company."));
                }

                var companyId = user.CompanyId.Value;
                var taxis = await _taxiService.GetTaxisAsync(companyId);

                return Ok(taxis);
            }
            catch (Exception)
            {
                return StatusCode(500, ResponseFactory.ErrorResponse(ResponseCodes.InternalServerError, ResponseMessages.InternalServerError));
            }
        }

        [HttpPut("EditTaxi/{id}")]
        public async Task<IActionResult> EditTaxi(int id, [FromBody] EditTaxiRequest model)
        {
            var success = await _taxiService.EditTaxiAsync(id, model);
            if (!success)
            {
                return NotFound(ResponseFactory.ErrorResponse(ResponseCodes.NotFound, "Taxi not found."));
            }

            return Ok(ResponseFactory.SuccessResponse("Taxi updated successfully.", success));
        }

        [HttpDelete("DeleteTaxi/{id}")]
        public async Task<IActionResult> DeleteTaxi(int id)
        {
            var success = await _taxiService.DeleteTaxiAsync(id);
            if (!success)
            {
                return NotFound(ResponseFactory.ErrorResponse(ResponseCodes.NotFound, "Taxi not found."));
            }

            return Ok(ResponseFactory.SuccessResponse("Taxi deleted successfully.", success));
        }

        private int? GetCompanyId()
        {
            return _httpContextAccessor.HttpContext?.Session?.GetInt32("UserId");
        }
    }
}