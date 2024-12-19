using Microsoft.AspNetCore.Mvc;
using pentasharp.Models.Enums;
using WebApplication1.Filters;
using pentasharp.Services;
using pentasharp.Models.TaxiRequest;
using pentasharp.Interfaces;
using pentasharp.ViewModel.TaxiModels;
using pentasharp.Models.Utilities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    [Route("Business/TaxiDriver")]
    [TypeFilter(typeof(BusinessOnlyFilter), Arguments = new object[] { "TaxiCompany" })]
    public class AddTaxiDriverController : Controller
    {
        private readonly ITaxiService _taxiService;
        private readonly IDriverService _driverService;
        private readonly IAuthenticateService _authService;
        private readonly ILogger<AddTaxiDriverController> _logger;

        public AddTaxiDriverController(
            ITaxiService taxiService,
            IDriverService driverService,
            IAuthenticateService authService,
            ILogger<AddTaxiDriverController> logger)
        {
            _taxiService = taxiService;
            _driverService = driverService;
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("AddDriver")]
        public async Task<IActionResult> AddDriver([FromBody] RegisterDriverRequest model)
        {
            _logger.LogInformation("Received request to add a new driver.");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid data provided for adding a new driver.");
                return BadRequest(new { success = false, message = "Invalid data provided." });
            }

            try
            {
                var newDriver = await _driverService.AddDriverAsync(model);
                _logger.LogInformation("Successfully added a new driver with ID: {DriverId}", newDriver.UserId);
                return Ok(new { success = true, message = "Driver added successfully.", driverId = newDriver.UserId });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Failed to add driver: {ErrorMessage}", ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while adding a new driver.");
                return StatusCode(500, new { success = false, message = "An unexpected error occurred." });
            }
        }

        [HttpGet("GetDrivers")]
        public IActionResult GetDrivers()
        {
            _logger.LogInformation("Received request to retrieve all drivers.");

            try
            {
                var drivers = _driverService.GetDrivers();

                _logger.LogInformation("Successfully retrieved drivers.");

                return Ok(drivers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while retrieving drivers.");
                return StatusCode(500, new { success = false, message = "An unexpected error occurred." });
            }
        }

        [HttpGet("GetAvailableDrivers/{taxiId?}")]
        public async Task<IActionResult> GetAvailableDrivers(int? taxiId = null)
        {
            _logger.LogInformation("Received request to retrieve available drivers for Taxi ID: {TaxiId}", taxiId);

            try
            {
                var drivers = await _taxiService.GetAvailableDriversAsync(taxiId);

                _logger.LogInformation("Successfully retrieved available drivers for Taxi ID: {TaxiId}", taxiId);

                return Ok(new { success = true, drivers });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while retrieving available drivers for Taxi ID: {TaxiId}", taxiId);
                return StatusCode(500, new { success = false, message = "An unexpected error occurred." });
            }
        }

        [HttpPut("EditDriver/{id}")]
        public async Task<IActionResult> EditDriver(int id, [FromBody] EditDriverRequest model)
        {
            _logger.LogInformation("Received request to edit driver with ID: {DriverId}", id);

            try
            {
                var success = await _driverService.EditDriverAsync(id, model);
                if (!success)
                {
                    _logger.LogWarning("Failed to edit driver with ID: {DriverId}. Driver not found or not authorized.", id);
                    return NotFound(new { success = false, message = "Driver not found or does not belong to your company." });
                }

                _logger.LogInformation("Successfully edited driver with ID: {DriverId}", id);
                return Ok(new { success = true, message = "Driver updated successfully." });
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while editing driver with ID: {DriverId}", id);
                return StatusCode(500, new { success = false, message = "An unexpected error occurred." });
            }
        }

        [HttpDelete("DeleteDriver/{id}")]
        public async Task<IActionResult> DeleteDriver(int id)
        {
            _logger.LogInformation("Received request to delete driver with ID: {DriverId}", id);

            try
            {
                var success = await _driverService.DeleteDriverAsync(id);
                if (!success)
                {
                    _logger.LogWarning("Failed to delete driver with ID: {DriverId}. Driver not found or not authorized.", id);
                    return NotFound(new { success = false, message = "Driver not found or does not belong to your company." });
                }

                _logger.LogInformation("Successfully deleted driver with ID: {DriverId}", id);
                return Ok(new { success = true, message = "Driver deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while deleting driver with ID: {DriverId}", id);
                return StatusCode(500, new { success = false, message = "An unexpected error occurred." });
            }
        }
    }
}