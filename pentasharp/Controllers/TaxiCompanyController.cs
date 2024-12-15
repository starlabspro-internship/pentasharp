using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pentasharp.Models.Enums;
using WebApplication1.Filters;
using pentasharp.Data;
using pentasharp.Services;
using pentasharp.Models.TaxiRequest;

namespace WebApplication1.Controllers
{
    [Route("Admin/TaxiCompany")]
    [ServiceFilter(typeof(AdminOnlyFilter))]
    public class TaxiCompanyController : Controller
    {
        private readonly ITaxiCompanyService _taxiCompanyService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;

        public TaxiCompanyController(
            ITaxiCompanyService taxiCompanyService,
            IHttpContextAccessor httpContextAccessor,
            AppDbContext context)
        {
            _taxiCompanyService = taxiCompanyService;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        [HttpPost("AddCompany")]
        public async Task<IActionResult> AddCompany([FromBody] TaxiCompanyRequest model)
        {
            if (ModelState.IsValid)
            {
                var company = await _taxiCompanyService.AddCompanyAsync(model);

                if (model.UserId != 0)
                {
                    var user = await _context.Users.FindAsync(model.UserId);
                    if (user != null)
                    {
                        user.CompanyId = company.TaxiCompanyId;
                        user.Role = UserRole.Admin;
                        if (user.Role == UserRole.Admin)
                        {
                            user.IsAdmin = true;
                        }
                        await _context.SaveChangesAsync();
                    }
                }
                return Ok(new { success = true, message = "Company added successfully." });
            }
            return BadRequest(new { success = false, message = "Invalid data provided." });
        }

        [HttpGet("GetCompanies")]
        public IActionResult GetCompanies()
        {
            var companies = _context.TaxiCompanies.Include(c => c.Taxis).ToList();
            var viewModel = _taxiCompanyService.GetAllCompanies();
            return Ok(viewModel);
        }

        [HttpGet("GetTaxiCompanyUsers")]
        public IActionResult GetTaxiCompanyUsers()
        {
            var users = _context.Users
                .Where(user => user.BusinessType == BusinessType.TaxiCompany)
                .Where(user => user.CompanyId == null)
                .Select(user => new
                {
                    user.UserId,
                    user.FirstName,
                    user.LastName
                })
                .ToList();

            return Ok(users);
        }

        [HttpGet("GetTaxiCompanyUser/{companyId}")]
        public IActionResult GetTaxiCompanyUser(int companyId)
        {
            var company = _context.TaxiCompanies
                .Where(tc => tc.TaxiCompanyId == companyId)
                .Select(tc => new
                {
                    User = _context.Users
                        .Where(u => u.CompanyId == companyId)
                        .Where(user => user.BusinessType == BusinessType.TaxiCompany)
                        .Select(u => new
                        {
                            u.UserId,
                            u.FirstName,
                            u.LastName
                        })
                        .FirstOrDefault()
                })
                .FirstOrDefault();

            if (company == null)
            {
                return NotFound(new { success = false, message = "Taxi Company not found." });
            }

            return Ok(new { success = true, data = company });
        }

        [HttpPut("EditCompany/{id}")]
        public async Task<IActionResult> EditCompany(int id, [FromBody] TaxiCompanyRequest model)
        {
            var result = await _taxiCompanyService.EditCompanyAsync(id, model);
            if (!result)
            {
                return NotFound(new { success = false, message = "Company not found." });
            }

            if (model.UserId != 0)
            {
                var user = await _context.Users.FindAsync(model.UserId);
                if (user != null)
                {
                    user.CompanyId = id;
                    user.Role = UserRole.Admin;
                    if (user.Role == UserRole.Admin)
                    {
                        user.IsAdmin = true;
                    }
                    await _context.SaveChangesAsync();
                }
            }

            return Ok(new { success = true, message = "Company updated successfully." });
        }

        [HttpDelete("DeleteCompany/{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var success = await _taxiCompanyService.DeleteCompanyAsync(id);
            if (!success)
            {
                return NotFound(new { success = false, message = "Company not found." });
            }

            return Ok(new { success = true, message = "Company and its taxis deleted successfully (soft delete)." });
        }
    }
}