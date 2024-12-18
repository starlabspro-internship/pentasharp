using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pentasharp.Data;
using pentasharp.Interfaces;
using pentasharp.Models.Entities;
using pentasharp.Models.Enums;
using pentasharp.Models.Utilities;
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
        private readonly ILogger<BusCompanyController> _logger;

        public BusCompanyController(IBusCompanyService companyService, IHttpContextAccessor httpContextAccessor, AppDbContext context, IMapper mapper, ILogger<BusCompanyController> logger)
        {
            _companyService = companyService;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("GetBusCompanyUsers")]
        public async Task<IActionResult> GetBusCompanyUsersAsync()
        {
            var users = await _companyService.GetBusCompanyUsersAsync();

            _logger.LogInformation("Successfully retrieved bus company users.");

            return Ok(users);
        }

        [HttpPost("AddCompany")]
        public async Task<IActionResult> AddCompanyAsync([FromBody] BusCompanyViewModel model)
        {
            var result = await _companyService.AddCompanyAsync(model);
            if (result)
            {
                _logger.LogInformation("Successfully added company: {CompanyName}", model.CompanyName);
                return Ok(ResponseFactory.SuccessResponse(ResponseMessages.Success, result));
            }

            _logger.LogWarning("Failed to add company: {CompanyName}. Invalid data provided.", model.CompanyName);
            return BadRequest(ResponseFactory.ErrorResponse(ResponseCodes.InvalidData, ResponseMessages.InvalidData));
        }

        [HttpPut("EditCompany/{id}")]
        public async Task<IActionResult> EditCompanyAsync(int id, [FromBody] BusCompanyViewModel model)
        {
            var result = await _companyService.EditCompanyAsync(id, model);
            if (result)
            {
                _logger.LogInformation("Successfully edited company with ID: {CompanyId}", id);
                return Ok(ResponseFactory.SuccessResponse(ResponseMessages.Success, result));
            }

            _logger.LogWarning("Failed to edit company with ID: {CompanyId}. Company not found.", id);
            return NotFound(ResponseFactory.ErrorResponse(ResponseCodes.NotFound, ResponseMessages.NotFound));
        }

        [HttpDelete("DeleteCompany/{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var result = await _companyService.DeleteCompanyAsync(id);
            if (result)
            {
                _logger.LogInformation("Successfully deleted company with ID: {CompanyId}", id);
                return Ok(ResponseFactory.SuccessResponse("Company and its buses deleted successfully.", result));
            }

            _logger.LogWarning("Failed to delete company with ID: {CompanyId}. Company not found.", id);
            return NotFound(ResponseFactory.ErrorResponse(ResponseCodes.NotFound, ResponseMessages.NotFound));
        }

        [HttpGet("GetCompanies")]
        public async Task<IActionResult> GetCompaniesAsync()
        {
            var companies = await _companyService.GetCompaniesAsync();

            _logger.LogInformation("Successfully retrieved {CompanyCount} companies.", companies.Count());

            return Ok(companies);
        }

        [HttpGet("GetBusCompanyUser/{companyId}")]
        public async Task<IActionResult> GetBusCompanyUserAsync(int companyId)
        {
            var response = await _companyService.GetBusCompanyUserAsync(companyId);

            if (response.Success)
            {
                _logger.LogInformation("Successfully retrieved users for company with ID: {CompanyId}", companyId);
                return Ok(ResponseFactory.SuccessResponse(ResponseMessages.Success, response.Data));
            }

            var errorMessage = response.Message.ToLower();

            if (errorMessage.Contains("not found"))
            {
                _logger.LogWarning("No users found for company with ID: {CompanyId}", companyId);
                return NotFound(ResponseFactory.ErrorResponse(ResponseCodes.NotFound, response.Message));
            }

            _logger.LogWarning("Invalid operation while retrieving users for company with ID: {CompanyId}. Message: {Message}", companyId, response.Message);
            return BadRequest(ResponseFactory.ErrorResponse(ResponseCodes.InvalidOperation, response.Message));
        }
    }
} 