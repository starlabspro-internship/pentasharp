using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pentasharp.Models.Enums;
using WebApplication1.Filters;
using pentasharp.Data;
using pentasharp.Services;
using pentasharp.Models.TaxiRequest;
using pentasharp.Models.Utilities;
using System.ComponentModel.Design;

namespace WebApplication1.Controllers
{
    [Route("Business/TaxiCompany")]
    [TypeFilter(typeof(BusinessOnlyFilter), Arguments = new object[] { "TaxiCompany" })]
    public class BusinessTaxiCompanyController : Controller
    {
        private readonly ITaxiService _taxiService;
        private readonly IDriverService _driverService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;
        private readonly ILogger<BusinessTaxiCompanyController> _logger;

        public BusinessTaxiCompanyController(
            ITaxiService taxiService,
            IDriverService driverService,
            IHttpContextAccessor httpContextAccessor,
            AppDbContext context,
            ILogger<BusinessTaxiCompanyController> logger)
        {
            _taxiService = taxiService;
            _driverService = driverService;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _logger = logger;
        }

        [HttpGet("GetCompany")]
        public IActionResult GetCompany()
        {
            _logger.LogInformation("Received request to retrieve the associated taxi company.");

            var company = _taxiService.GetCompanyDetails();
            if (company == null)
            {
                _logger.LogWarning("No associated taxi company found for the current user.");
                return NotFound(ResponseFactory.ErrorResponse(ResponseCodes.NotFound, "No associated taxi company found for this user."));
            }

            _logger.LogInformation("Successfully retrieved the associated taxi company.");
            return Ok(company);
        }

        [HttpPost("AddTaxi")]
        public async Task<IActionResult> AddTaxi([FromBody] AddTaxiRequest model)
        {
            _logger.LogInformation("Received request to add a new taxi.");

            var taxi = await _taxiService.AddTaxiAsync(model);

            _logger.LogInformation("Taxi added successfully with ID: {TaxiId}", taxi.TaxiId);
            return Ok(ResponseFactory.SuccessResponse("Taxi added successfully.", taxi));
        }

        [HttpGet("GetTaxis")]
        public async Task<IActionResult> GetTaxis()
        {
            _logger.LogInformation("Received request to retrieve all taxis.");

            var taxis = await _taxiService.GetTaxisAsync();

            if (taxis != null && taxis.Any())
            {
                _logger.LogInformation("Successfully retrieved {Count} taxis.", taxis.Count);
                return Ok(taxis);
            }

            _logger.LogWarning("No taxis found.");
            return NotFound(ResponseFactory.ErrorResponse(ResponseCodes.NotFound, "No taxis found."));
        }

        [HttpPut("EditTaxi/{id}")]
        public async Task<IActionResult> EditTaxi(int id, [FromBody] EditTaxiRequest model)
        {
            _logger.LogInformation("Received request to edit taxi with ID: {TaxiId}", id);

            var success = await _taxiService.EditTaxiAsync(id, model);
            if (!success)
            {
                _logger.LogWarning("Failed to update taxi. Taxi with ID: {TaxiId} not found.", id);
                return NotFound(ResponseFactory.ErrorResponse(ResponseCodes.NotFound, "Taxi not found."));
            }

            _logger.LogInformation("Taxi with ID: {TaxiId} updated successfully.", id);
            return Ok(ResponseFactory.SuccessResponse("Taxi updated successfully.", success));
        }

        [HttpDelete("DeleteTaxi/{id}")]
        public async Task<IActionResult> DeleteTaxi(int id)
        {
            _logger.LogInformation("Received request to delete taxi with ID: {TaxiId}", id);

            var success = await _taxiService.DeleteTaxiAsync(id);
            if (!success)
            {
                _logger.LogWarning("Failed to delete taxi. Taxi with ID: {TaxiId} not found.", id);
                return NotFound(ResponseFactory.ErrorResponse(ResponseCodes.NotFound, "Taxi not found."));
            }

            _logger.LogInformation("Taxi with ID: {TaxiId} deleted successfully.", id);
            return Ok(ResponseFactory.SuccessResponse("Taxi deleted successfully.", success));
        }
    }
}