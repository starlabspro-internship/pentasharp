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

        public BusinessBusCompanyController(
            IBusService busService,
            IHttpContextAccessor httpContextAccessor,
            AppDbContext context,
            IMapper mapper,
            IBusCompanyService companyService)
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
            var company = await _companyService.GetCompanyByUserIdAsync();
            if (company != null)

                return Ok(company);

            return NotFound(ResponseFactory.ErrorResponse(ResponseCodes.NotFound, "No company associated with the logged-in user."));
        }

        [HttpPost("AddBus")]
        public async Task<IActionResult> AddBusAsync([FromBody] AddBusViewModel model)
        {
            var result = await _busService.AddBusAsync(model);
            if (result)
                return Ok(ResponseFactory.SuccessResponse(ResponseMessages.Success, result));

            return BadRequest(ResponseFactory.ErrorResponse(ResponseCodes.InvalidData, ResponseMessages.InvalidData));
        }

        [HttpGet("GetBuses")]
        public async Task<IActionResult> GetBusesAsync()
        {
            var buses = await _busService.GetBusesAsync();
            if (buses != null)
                return Ok(buses);

            return NotFound(ResponseFactory.ErrorResponse(ResponseCodes.NotFound, ResponseMessages.NotFound));
        }

        [HttpPut("EditBus/{id}")]
        public async Task<IActionResult> EditBusAsync(int id, [FromBody] EditBusViewModel model)
        {
            var result = await _busService.EditBusAsync(id, model);
            if (result)
                return Ok(ResponseFactory.SuccessResponse("Bus updated successfully.",result));

            return NotFound(ResponseFactory.ErrorResponse(ResponseCodes.NotFound, "Bus not found."));
        }

        [HttpDelete("DeleteBus/{id}")]
        public async Task<IActionResult> DeleteBusAsync(int id)
        {
            var result = await _busService.DeleteBusAsync(id);
            if (result)
                return Ok(ResponseFactory.SuccessResponse("Bus deleted successfully.", result));

            return NotFound(ResponseFactory.ErrorResponse(ResponseCodes.NotFound, "Bus not found."));
        }
    }
}