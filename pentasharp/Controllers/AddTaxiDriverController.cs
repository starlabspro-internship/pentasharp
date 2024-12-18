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
    public class AddTaxiDriverController : Controller
    {
        private readonly ITaxiService _taxiService;
        private readonly IDriverService _driverService;
        private readonly IAuthenticateService _authService;

        public AddTaxiDriverController(
            ITaxiService taxiService,
            IDriverService driverService,
            IAuthenticateService authService)
        {
            _taxiService = taxiService;
            _driverService = driverService;
            _authService = authService;
        }

        [HttpPost("AddDriver")]
        public async Task<IActionResult> AddDriver([FromBody] RegisterDriverRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Invalid data provided." });
            }

            try
            {
                var newDriver = await _driverService.AddDriverAsync(model);
                return Ok(new { success = true, message = "Driver added successfully.", driverId = newDriver.UserId });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { success = false, message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { success = false, message = "An unexpected error occurred." });
            }
        }

        [HttpGet("GetDrivers")]
        public IActionResult GetDrivers()
        {
            try
            {
                var drivers = _driverService.GetDrivers();
                return Ok(drivers);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { success = false, message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { success = false, message = "An unexpected error occurred." });
            }
        }

        [HttpGet("GetAvailableDrivers/{taxiId?}")]
        public async Task<IActionResult> GetAvailableDrivers(int? taxiId = null)
        {
            try
            {
                var drivers = await _taxiService.GetAvailableDriversAsync(taxiId);
                return Ok(new { success = true, drivers });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { success = false, code = ApiStatusEnum.UNAUTHORIZED, message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { success = false, message = "An unexpected error occurred." });
            }
        }

        [HttpPut("EditDriver/{id}")]
        public async Task<IActionResult> EditDriver(int id, [FromBody] EditDriverRequest model)
        {
            try
            {
                var success = await _driverService.EditDriverAsync(id, model);
                if (!success)
                {
                    return NotFound(new { success = false, message = "Driver not found or does not belong to your company." });
                }

                return Ok(new { success = true, message = "Driver updated successfully." });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { success = false, message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { success = false, message = "An unexpected error occurred." });
            }
        }

        [HttpDelete("DeleteDriver/{id}")]
        public async Task<IActionResult> DeleteDriver(int id)
        {
            try
            {
                var success = await _driverService.DeleteDriverAsync(id);
                if (!success)
                {
                    return NotFound(new { success = false, message = "Driver not found or does not belong to your company." });
                }

                return Ok(new { success = true, message = "Driver deleted successfully." });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { success = false, message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { success = false, message = "An unexpected error occurred." });
            }
        }
    }
}