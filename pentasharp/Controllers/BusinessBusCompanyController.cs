using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pentasharp.Data;
using pentasharp.Interfaces;
using pentasharp.Models.Enums;
using pentasharp.Models.Utilities;
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
        private readonly ILogger<BusinessBusCompanyController> _logger;

        public BusinessBusCompanyController(
            IBusService busService,
            IHttpContextAccessor httpContextAccessor,
            AppDbContext context,
            IMapper mapper,
            IBusCompanyService companyService,
            ILogger<BusinessBusCompanyController> logger)
        {
            _busService = busService;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mapper = mapper;
            _companyService = companyService;
            _logger = logger;
        }

        [HttpGet("GetCompany")]
        public async Task<IActionResult> GetCompanyAsync()
        {
            var company = await _companyService.GetCompanyByUserIdAsync();

            if (company != null)
            {
                _logger.LogInformation("Successfully retrieved the bus company associated with the user.");
                return Ok(company);
            }

            _logger.LogWarning("No company associated with the logged-in user.");
            return NotFound(ResponseFactory.ErrorResponse(ResponseCodes.NotFound, "No company associated with the logged-in user."));
        }

        [HttpPost("AddBus")]
        public async Task<IActionResult> AddBusAsync([FromBody] AddBusViewModel model)
        {
            _logger.LogInformation("Received request to add a new bus.");

            var result = await _busService.AddBusAsync(model);

            if (result)
            {
                _logger.LogInformation("Bus added successfully.");
                return Ok(ResponseFactory.SuccessResponse(ResponseMessages.Success, result));
            }

            _logger.LogWarning("Failed to add bus due to invalid data.");
            return BadRequest(ResponseFactory.ErrorResponse(ResponseCodes.InvalidData, ResponseMessages.InvalidData));
        }

        [HttpGet("GetBuses")]
        public async Task<IActionResult> GetBusesAsync()
        {
            _logger.LogInformation("Received request to retrieve all buses.");

            var buses = await _busService.GetBusesAsync();

            if (buses != null)
            {
                _logger.LogInformation("Successfully retrieved buses.");
                return Ok(buses);
            }

            _logger.LogWarning("No buses found.");
            return NotFound(ResponseFactory.ErrorResponse(ResponseCodes.NotFound, ResponseMessages.NotFound));
        }

        [HttpPut("EditBus/{id}")]
        public async Task<IActionResult> EditBusAsync(int id, [FromBody] EditBusViewModel model)
        {
            _logger.LogInformation("Received request to edit bus with ID: {BusId}", id);

            var result = await _busService.EditBusAsync(id, model);

            if (result)
            {
                _logger.LogInformation("Bus with ID: {BusId} updated successfully.", id);
                return Ok(ResponseFactory.SuccessResponse("Bus updated successfully.", result));
            }

            _logger.LogWarning("Failed to update bus. Bus with ID: {BusId} not found.", id);
            return NotFound(ResponseFactory.ErrorResponse(ResponseCodes.NotFound, "Bus not found."));
        }

        [HttpDelete("DeleteBus/{id}")]
        public async Task<IActionResult> DeleteBusAsync(int id)
        {
            _logger.LogInformation("Received request to delete bus with ID: {BusId}", id);

            var result = await _busService.DeleteBusAsync(id);

            if (result)
            {
                _logger.LogInformation("Bus with ID: {BusId} deleted successfully.", id);
                return Ok(ResponseFactory.SuccessResponse("Bus deleted successfully.", result));
            }

            _logger.LogWarning("Failed to delete bus. Bus with ID: {BusId} not found.", id);
            return NotFound(ResponseFactory.ErrorResponse(ResponseCodes.NotFound, "Bus not found."));
        }
    }
}