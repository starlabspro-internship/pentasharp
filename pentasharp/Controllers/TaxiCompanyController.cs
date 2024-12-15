using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pentasharp.Models.Enums;
using WebApplication1.Filters;
using pentasharp.Data;
using pentasharp.Services;
using pentasharp.Models.TaxiRequest;
using pentasharp.Models.Utilities;
using System.Threading.Tasks;
using System.Linq;

namespace WebApplication1.Controllers
{
    [Route("Admin/TaxiCompany")]
    [ServiceFilter(typeof(AdminOnlyFilter))]
    public class TaxiCompanyController : Controller
    {
        private readonly ITaxiCompanyService _taxiCompanyService;

        public TaxiCompanyController(
            ITaxiCompanyService taxiCompanyService)
        {
            _taxiCompanyService = taxiCompanyService;
        }

        [HttpPost("AddCompany")]
        public async Task<IActionResult> AddCompany([FromBody] TaxiCompanyRequest model)
        {
            if (ModelState.IsValid)
            {
                var result = await _taxiCompanyService.AddCompanyAndAssignUserAsync(model);
                if (result)
                    return Ok(ResponseFactory.SuccessResponse("Company added successfully." ,result));

                return BadRequest(ResponseFactory.ErrorResponse(ResponseCodes.InvalidData, "Invalid data provided or operation failed."));
            }
            return BadRequest(ResponseFactory.ErrorResponse(ResponseCodes.InvalidData, ResponseMessages.InvalidData));
        }

        [HttpGet("GetCompanies")]
        public IActionResult GetCompanies()
        {
            var viewModel = _taxiCompanyService.GetAllCompaniesWithTaxis();
            return Ok(viewModel);
        }

        [HttpGet("GetTaxiCompanyUsers")]
        public IActionResult GetTaxiCompanyUsers()
        {
            var users = _taxiCompanyService.GetUnassignedTaxiCompanyUsers();
            return Ok(users);
        }

        [HttpGet("GetTaxiCompanyUser/{companyId}")]
        public IActionResult GetTaxiCompanyUser(int companyId)
        {
            var company = _taxiCompanyService.GetTaxiCompanyUser(companyId);
            if (company == null)
            {
                return NotFound(ResponseFactory.ErrorResponse(ResponseCodes.NotFound, "Taxi Company not found."));
            }

            return Ok(new { success = true, data = company });
        }

        [HttpPut("EditCompany/{id}")]
        public async Task<IActionResult> EditCompany(int id, [FromBody] TaxiCompanyRequest model)
        {
            var result = await _taxiCompanyService.EditCompanyAndAssignUserAsync(id, model);
            if (!result)
            {
                return NotFound(ResponseFactory.ErrorResponse(ResponseCodes.NotFound, "Company not found."));
            }

            return Ok(ResponseFactory.SuccessResponse("Company updated successfully.",result));
        }

        [HttpDelete("DeleteCompany/{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var success = await _taxiCompanyService.DeleteCompanyAsync(id);
            if (!success)
            {
                return NotFound(ResponseFactory.ErrorResponse(ResponseCodes.NotFound, "Company not found."));
            }

            return Ok(ResponseFactory.SuccessResponse("Company and its taxis deleted successfully (soft delete).", success));
        }
    }
}