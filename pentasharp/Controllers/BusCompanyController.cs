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
    [Route("Admin/BusCompany")]
    [ServiceFilter(typeof(AdminOnlyFilter))]
    public class BusCompanyController : Controller
    {
        private readonly IBusCompanyService _companyService;
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public BusCompanyController(IBusCompanyService companyService, IHttpContextAccessor httpContextAccessor, AppDbContext context, IMapper mapper)
        {
            _companyService = companyService;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mapper = mapper;
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

        private int? GetUserId()
        {
            return _httpContextAccessor.HttpContext?.Session?.GetInt32("UserId");
        }
    }
} 