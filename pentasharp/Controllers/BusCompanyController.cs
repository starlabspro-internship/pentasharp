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
    [Route("api/BusCompany")]
    [ServiceFilter(typeof(AdminOnlyFilter))]
    public class BusCompanyController : Controller
    {
        private readonly IBusCompanyService _companyService;
        private readonly IBusService _busService;
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public BusCompanyController(IBusCompanyService companyService, IBusService busService, IHttpContextAccessor httpContextAccessor, AppDbContext context, IMapper mapper)
        {
            _companyService = companyService;
            _busService = busService;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("Bus")]
        public IActionResult Add()
        {
            var companies = _context.BusCompanies.ToList();

            var viewModel = new ManageBusCompanyViewModel
            {
                BusCompanies = _mapper.Map<List<BusCompanyViewModel>>(companies),
            };
            return View(viewModel);
        }

        [HttpGet("GetBusCompanyUsers")]
        public async Task<IActionResult> GetBusCompanyUsersAsync()
        {
            var users = await _companyService.GetBusCompanyUsersAsync();
            if (users != null)
                return Ok(users);

            return NotFound(new { success = false, message = "No users found." });
        }

        [HttpPost("AddCompany")]
        public async Task<IActionResult> AddCompanyAsync([FromBody] BusCompanyViewModel model)
        {
            var result = await _companyService.AddCompanyAsync(model);
            if (result)
                return Ok(new { success = true, message = "Company added successfully." });

            return BadRequest(new { success = false, message = "Invalid data provided." });
        }

        [HttpPut("EditCompany/{id}")]
        public async Task<IActionResult> EditCompanyAsync(int id, [FromBody] BusCompanyViewModel model)
        {
            var result = await _companyService.EditCompanyAsync(id, model);
            if (result)
                return Ok(new { success = true, message = "Company updated successfully." });

            return NotFound(new { success = false, message = "Company not found." });
        }

        [HttpDelete("DeleteCompany/{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var result = await _companyService.DeleteCompanyAsync(id);
            if (result)
                return Ok(new { success = true, message = "Company and its buses deleted successfully." });

            return NotFound(new { success = false, message = "Company not found." });
        }

        [HttpGet("GetCompanies")]
        public async Task<IActionResult> GetCompaniesAsync()
        {
            var companies = await _companyService.GetCompaniesAsync();
            return Ok(companies);
        }

        [HttpGet("GetBusCompanyUser/{companyId}")]
        public async Task<IActionResult> GetBusCompanyUserAsync(int companyId)
        {
            var user = await _companyService.GetBusCompanyUserAsync(companyId);
            if (user != null)
                return Ok(new { success = true, data = user });

            return NotFound(new { success = false, message = "Bus Company not found." });
        }

        [HttpGet("GetCompany")]
        public async Task<IActionResult> GetCompanyAsync()
        {
            var userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserId");
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
            var userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserId");
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
            var userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserId");
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
            var userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserId");
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
            var userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return Unauthorized(new { success = false, message = "User not logged in." });

            var result = await _busService.DeleteBusAsync(id, userId.Value);
            if (result)
                return Ok(new { success = true, message = "Bus deleted successfully." });

            return NotFound(new { success = false, message = "Bus not found." });
        }
    }
}