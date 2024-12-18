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

            return NotFound(ResponseFactory.ErrorResponse(ResponseCodes.NotFound, ResponseMessages.NotFound));
        }

        [HttpPost("AddCompany")]
        public async Task<IActionResult> AddCompanyAsync([FromBody] BusCompanyViewModel model)
        {
            var result = await _companyService.AddCompanyAsync(model);
            if (result)
            {
                return Ok(ResponseFactory.SuccessResponse(ResponseMessages.Success, result));
            }

            return BadRequest(ResponseFactory.ErrorResponse(ResponseCodes.InvalidData, ResponseMessages.InvalidData));
        }

        [HttpPut("EditCompany/{id}")]
        public async Task<IActionResult> EditCompanyAsync(int id, [FromBody] BusCompanyViewModel model)
        {
            var result = await _companyService.EditCompanyAsync(id, model);
            if (result)
                return Ok(ResponseFactory.SuccessResponse(ResponseMessages.Success, result));

            return NotFound(ResponseFactory.ErrorResponse(ResponseCodes.NotFound, ResponseMessages.NotFound));
        }

        [HttpDelete("DeleteCompany/{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var result = await _companyService.DeleteCompanyAsync(id);
            if (result)
                return Ok(ResponseFactory.SuccessResponse("Company and its buses deleted successfully.",result));

            return NotFound(ResponseFactory.ErrorResponse(ResponseCodes.NotFound, ResponseMessages.NotFound));
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
            var response = await _companyService.GetBusCompanyUserAsync(companyId);

            if (response.Success)
            {
                return Ok(ResponseFactory.SuccessResponse(ResponseMessages.Success, response.Data));
            }
            else
            {
                if (response.Message.Contains("not found", System.StringComparison.OrdinalIgnoreCase))
                {
                    return NotFound(ResponseFactory.ErrorResponse(ResponseCodes.NotFound, response.Message));
                }
                else
                {
                    return BadRequest(ResponseFactory.ErrorResponse(ResponseCodes.InvalidOperation, response.Message));
                }
            }
        }
    }
} 