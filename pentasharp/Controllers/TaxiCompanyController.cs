using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pentasharp.Models.Enums;
using WebApplication1.Filters;
using pentasharp.Data;
using pentasharp.Services;
using pentasharp.Models.TaxiRequest;

namespace WebApplication1.Controllers
{
    [Route("api/TaxiCompany")]
    [ServiceFilter(typeof(AdminOnlyFilter))]
    public class TaxiCompanyController : Controller
    {
        private readonly ITaxiCompanyService _taxiCompanyService;
        private readonly ITaxiService _taxiService;
        private readonly IDriverService _driverService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;

        public TaxiCompanyController(
            ITaxiCompanyService taxiCompanyService,
            ITaxiService taxiService,
            IDriverService driverService,
            IHttpContextAccessor httpContextAccessor,
            AppDbContext context)
        {
            _taxiCompanyService = taxiCompanyService;
            _taxiService = taxiService;
            _driverService = driverService;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        [HttpGet("Taxi")]
        public IActionResult Add()
        {
            var companies = _taxiCompanyService.GetAllCompanies();
            var viewModel = new ManageTaxiCompanyRequest
            {
                TaxiCompanies = companies,
            };
            return View(viewModel);
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

        [HttpGet("GetCompany")]
        public IActionResult GetCompany()
        {
            var userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserID");
            if (!userId.HasValue)
            {
                return Unauthorized("No user is logged in.");
            }

            var user = _context.Users.Find(userId.Value);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (user.BusinessType == BusinessType.TaxiCompany)
            {
                var company = _context.TaxiCompanies
                    .FirstOrDefault(c => c.UserId == userId.Value);

                if (company == null)
                {
                    return NotFound("No associated taxi company found for this user.");
                }

                return Ok(company);
            }
            else
            {
                return Ok("User is not associated with Taxi Company.");
            }
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

        [HttpPost("AddTaxi")]
        public async Task<IActionResult> AddTaxi([FromBody] AddTaxiRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, code = ResponseCodes.InvalidData, message = "Invalid data provided." });
            }

            bool isDriverAssigned = await _context.Taxis.AnyAsync(t => t.DriverId == model.DriverId && model.DriverId != null && !t.IsDeleted);
            if (isDriverAssigned)
            {
                return Conflict(new { success = false, message = "Driver is already assigned to another taxi." });
            }

            var taxi = await _taxiService.AddTaxiAsync(model);
            return Ok(new { success = true, code = ResponseCodes.Success, message = "Taxi added successfully." });
        }

        [HttpGet("GetTaxis")]
        public async Task<IActionResult> GetTaxis()
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserID");
                if (!userId.HasValue)
                {
                    return Unauthorized(new { success = false, message = "No user is logged in." });
                }

                var user = await _context.Users.FindAsync(userId.Value);
                if (user == null)
                {
                    return NotFound(new { success = false, message = "User not found." });
                }

                if (!user.CompanyId.HasValue)
                {
                    return NotFound(new { success = false, message = "The logged-in user has no associated company." });
                }

                var companyId = user.CompanyId.Value;
                var taxis = await _taxiService.GetTaxisAsync(companyId);

                return Ok(taxis);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An unexpected error occurred." });
            }
        }

        [HttpPut("EditTaxi/{id}")]
        public async Task<IActionResult> EditTaxi(int id, [FromBody] EditTaxiRequest model)
        {
            var success = await _taxiService.EditTaxiAsync(id, model);
            if (!success)
            {
                return NotFound(new { success = false, message = "Taxi not found." });
            }

            return Ok(new { success = true, message = "Taxi updated successfully." });
        }

        [HttpDelete("DeleteTaxi/{id}")]
        public async Task<IActionResult> DeleteTaxi(int id)
        {
            var success = await _taxiService.DeleteTaxiAsync(id);
            if (!success)
            {
                return NotFound(new { success = false, message = "Taxi not found." });
            }

            return Ok(new { success = true, message = "Taxi deleted successfully." });
        }

        [HttpPost("AddDriver")]
        public async Task<IActionResult> AddDriver([FromBody] RegisterDriverRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Invalid data provided." });
            }

            var userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserID");
            if (!userId.HasValue)
            {
                return Unauthorized("No user is logged in.");
            }

            var user = await _context.Users.FindAsync(userId.Value);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var company = await _context.TaxiCompanies.FirstOrDefaultAsync(c => c.UserId == user.UserId);
            if (company == null)
            {
                return NotFound("No associated taxi company found for this user.");
            }

            var newDriver = await _driverService.AddDriverAsync(model, company.TaxiCompanyId);
            return Ok(new { success = true, message = "Driver added successfully." });
        }

        [HttpGet("GetDrivers")]
        public IActionResult GetDrivers()
        {
            var userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserID");
            if (!userId.HasValue)
            {
                return Unauthorized("No user is logged in.");
            }

            var user = _context.Users.Find(userId.Value);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var company = _context.TaxiCompanies.FirstOrDefault(c => c.UserId == user.UserId);
            if (company == null)
            {
                return NotFound("No associated taxi company found for this user.");
            }

            var drivers = _driverService.GetDrivers(company.TaxiCompanyId);
            return Ok(drivers);
        }

        [HttpGet("GetAvailableDrivers/{taxiId?}")]
        public async Task<IActionResult> GetAvailableDrivers(int? taxiId = null)
        {
            var userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserID");
            if (!userId.HasValue)
            {
                return Unauthorized(new { success = false, code = ApiStatusEnum.UNAUTHORIZED, message = "No user is logged in." });
            }

            var user = await _context.Users.FindAsync(userId.Value);
            if (user == null)
            {
                return NotFound(new { success = false, code = ResponseCodes.NotFound, message = "User not found." });
            }

            var company = await _context.TaxiCompanies.FirstOrDefaultAsync(c => c.UserId == user.UserId);
            if (company == null)
            {
                return NotFound(new { success = false, code = ResponseCodes.NotFound, message = "No associated taxi company found for this user." });
            }

            var drivers = await _taxiService.GetAvailableDriversAsync(company.TaxiCompanyId, taxiId);
            return Ok(new { success = true, drivers });
        }

        [HttpPut("EditDriver/{id}")]
        public async Task<IActionResult> EditDriver(int id, [FromBody] EditDriverRequest model)
        {
            var success = await _driverService.EditDriverAsync(id, model);
            if (!success)
            {
                return NotFound(new { success = false, message = "Driver not found." });
            }

            return Ok(new { success = true, message = "Driver updated successfully." });
        }

        [HttpDelete("DeleteDriver/{id}")]
        public async Task<IActionResult> DeleteDriver(int id)
        {
            var success = await _driverService.DeleteDriverAsync(id);
            if (!success)
            {
                return NotFound(new { success = false, message = "Driver not found." });
            }

            return Ok(new { success = true, message = "Driver deleted successfully." });
        }
    }
}