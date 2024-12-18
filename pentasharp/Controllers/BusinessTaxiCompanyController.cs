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
            var company = _taxiService.GetCompanyDetails();
            if (company == null)
            {
                return NotFound(ResponseFactory.ErrorResponse(ResponseCodes.NotFound, "No associated taxi company found for this user."));
            }

            return Ok(company);
        }

        [HttpPost("AddTaxi")]
        public async Task<IActionResult> AddTaxi([FromBody] AddTaxiRequest model)
        {
            var taxi = await _taxiService.AddTaxiAsync(model);
            return Ok(ResponseFactory.SuccessResponse("Taxi added successfully.",taxi));
        }

        [HttpGet("GetTaxis")]
        public async Task<IActionResult> GetTaxis()
        {
            var taxis = await _taxiService.GetTaxisAsync();
            return Ok(taxis);
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
    }
}