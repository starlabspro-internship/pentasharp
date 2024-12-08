using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using pentasharp.Data;
using pentasharp.Models.Entities;
using pentasharp.Models.Enums;
using pentasharp.ViewModel.Authenticate;
using pentasharp.ViewModel.Taxi;
using pentasharp.ViewModel.TaxiModels; 
using WebApplication1.Filters;
using System.Text;

namespace WebApplication1.Controllers
{
    [Route("api/TaxiCompany")]
    [ServiceFilter(typeof(AdminOnlyFilter))]
    public class TaxiCompanyController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TaxiCompanyController(AppDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("Taxi")]
        public IActionResult Add()
        {
            var companies = _context.TaxiCompanies.ToList();
            var viewModel = new ManageTaxiCompanyViewModel
            {
                TaxiCompanies = _mapper.Map<List<TaxiCompanyViewModel>>(companies),
            };
            return View(viewModel);
        }

        [HttpPost("AddCompany")]
        public IActionResult AddCompany([FromBody] TaxiCompanyViewModel model)
        {
            if (ModelState.IsValid)
            {
                var company = _mapper.Map<TaxiCompany>(model);
                _context.TaxiCompanies.Add(company);
                _context.SaveChanges();

                if (model.UserId != 0)
                {
                    var user = _context.Users.Find(model.UserId);
                    if (user != null)
                    {
                        user.CompanyId = company.TaxiCompanyId;
                        user.Role = UserRole.Admin;
                        if (user.Role == UserRole.Admin)
                        {
                            user.IsAdmin = true;
                        }
                        _context.SaveChanges();
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
            var viewModel = _mapper.Map<List<TaxiCompanyViewModel>>(companies);
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
                    .Where(c => c.UserId == userId.Value)
                    .FirstOrDefault();

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

        [ServiceFilter(typeof(AdminOnlyFilter))]
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

        [ServiceFilter(typeof(AdminOnlyFilter))]
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
        public IActionResult EditCompany(int id, [FromBody] TaxiCompanyViewModel model)
        {
            var company = _context.TaxiCompanies.Find(id);
            if (company == null)
            {
                return NotFound(new { success = false, message = "Company not found." });
            }
            if (model.UserId != 0)
            {
                var user = _context.Users.Find(model.UserId);
                if (user != null)
                {
                    user.CompanyId = company.TaxiCompanyId;
                    user.Role = UserRole.Admin;
                    if (user.Role == UserRole.Admin)
                    {
                        user.IsAdmin = true;
                    }
                    _context.SaveChanges();
                }
            }

            _mapper.Map(model, company);
            _context.SaveChanges();
            return Ok(new { success = true, message = "Company updated successfully." });
        }

        [HttpDelete("DeleteCompany/{id}")]
        public IActionResult DeleteCompany(int id)
        {
            var company = _context.TaxiCompanies
                                  .Include(c => c.Taxis)
                                  .FirstOrDefault(c => c.TaxiCompanyId == id);

            if (company == null)
            {
                return NotFound(new { success = false, message = "Company not found." });
            }

            company.IsDeleted = true;
            company.UpdatedAt = DateTime.UtcNow;

            foreach (var taxi in company.Taxis)
            {
                taxi.IsDeleted = true;
                taxi.UpdatedAt = DateTime.UtcNow;
            }

            _context.TaxiCompanies.Update(company);
            _context.Taxis.UpdateRange(company.Taxis);
            _context.SaveChanges();

            return Ok(new { success = true, message = "Company and its taxis deleted successfully (soft delete)." });
        }

        [HttpPost("AddTaxi")]
        public async Task<IActionResult> AddTaxi([FromBody] AddTaxiViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, code = ResponseCodes.InvalidData, message = "Invalid data provided." });
            }

            bool isDriverAssigned = await _context.Taxis.AnyAsync(t => t.DriverId == model.DriverId && !t.IsDeleted);
            if (isDriverAssigned)
            {
                return Conflict(new { success = false, code = ResponseCodes.Conflict, message = "Driver is already assigned to another taxi." });
            }

            var taxi = _mapper.Map<Taxi>(model);
            _context.Taxis.Add(taxi);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, code = ResponseCodes.Success, message = "Taxi added successfully." });
        }

        [HttpGet("GetTaxis")]
        public IActionResult GetTaxis()
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

            if (!user.CompanyId.HasValue)
            {
                return NotFound("The logged-in user has no associated company.");
            }

            var companyId = user.CompanyId.Value;

            var taxis = _context.Taxis
                .Include(t => t.TaxiCompany)
                .Include(t => t.Driver)
                .Where(t => t.TaxiCompanyId == companyId && !t.IsDeleted)
                .ToList();

            var viewModel = taxis.Select(t => new TaxiViewModel
            {
                TaxiId = t.TaxiId,
                LicensePlate = t.LicensePlate,
                TaxiCompanyId = t.TaxiCompanyId,
                CompanyName = t.TaxiCompany?.CompanyName,
                DriverId = t.DriverId,
                DriverName = t.DriverId.HasValue
                ? _context.Users
                    .Where(u => u.UserId == t.DriverId.Value)
                    .Select(u => $"{u.FirstName} {u.LastName}")
                    .FirstOrDefault()
                : "No Driver Assigned"

            }).ToList();

            return Ok(viewModel);
        }

        [HttpPut("EditTaxi/{id}")]
        public IActionResult EditTaxi(int id, [FromBody] EditTaxiViewModel model)
        {
            try
            {
                var taxi = _context.Taxis.Find(id);
                if (taxi == null)
                {
                    return NotFound(new { success = false, message = "Taxi not found." });
                }

                _mapper.Map(model, taxi);
                _context.SaveChanges();
                return Ok(new { success = true, message = "Taxi updated successfully." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in EditTaxi: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An error occurred while updating the taxi." });
            }
        }


        [HttpDelete("DeleteTaxi/{id}")]
        public IActionResult DeleteTaxi(int id)
        {
            var taxi = _context.Taxis.Find(id);
            if (taxi == null)
            {
                return NotFound(new { success = false, message = "Taxi not found." });
            }

            taxi.IsDeleted = true;

            _context.Taxis.Remove(taxi);
            _context.SaveChanges();
            return Ok(new { success = true, message = "Taxi deleted successfully." });
        }

        [HttpPost("AddDriver")]
        public IActionResult AddDriver([FromBody] RegisterDriverViewModel model)
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

            var newUser = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PasswordHash = HashPassword(model.Password),
                CompanyId = company.TaxiCompanyId,
                Role = UserRole.Driver,
                IsAdmin = false,
                BusinessType = BusinessType.TaxiCompany
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return Ok(new { success = true, message = "Driver added successfully." });
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
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

            var drivers = _context.Users
                .Where(u => u.CompanyId == company.TaxiCompanyId && u.Role == UserRole.Driver && !u.IsDeleted)
                .Select(u => new
                {
                    u.UserId,
                    u.FirstName,
                    u.LastName,
                    u.Email,
                    CompanyName = company.CompanyName
                })
                .ToList();

            return Ok(drivers);
        }

        [HttpGet("GetAvailableDrivers/{taxiId?}")]
        public IActionResult GetAvailableDrivers(int? taxiId = null)
        {
            var userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserID");
            if (!userId.HasValue)
            {
                return Unauthorized(new { success = false, code = ApiStatusEnum.UNAUTHORIZED, message = "No user is logged in." });
            }

            var user = _context.Users.Find(userId.Value);
            if (user == null)
            {
                return NotFound(new { success = false, code = ResponseCodes.NotFound, message = "User not found." });
            }

            var company = _context.TaxiCompanies.FirstOrDefault(c => c.UserId == user.UserId);
            if (company == null)
            {
                return NotFound(new { success = false, code = ResponseCodes.NotFound, message = "No associated taxi company found for this user." });
            }

            var assignedDriverIds = _context.Taxis
                .Where(t => t.TaxiCompanyId == company.TaxiCompanyId && !t.IsDeleted && (taxiId == null || t.TaxiId != taxiId))
                .Select(t => t.DriverId)
                .ToList();

            var drivers = _context.Users
                .Where(u => u.CompanyId == company.TaxiCompanyId
                            && u.Role == UserRole.Driver
                            && !u.IsDeleted
                            && !assignedDriverIds.Contains(u.UserId))
                .Select(u => new {
                    u.UserId,
                    u.FirstName,
                    u.LastName,
                    u.Email,
                    CompanyName = company.CompanyName
                })
                .ToList();

            return Ok(new { success = true, drivers });
        }

        [HttpPut("EditDriver/{id}")]
        public IActionResult EditDriver(int id, [FromBody] EditDriverViewModel model)
        {
            var user = _context.Users.Find(id);
            if (user == null || user.IsDeleted || user.Role != UserRole.Driver)
            {
                return NotFound(new { success = false, message = "Driver not found." });
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            if (!string.IsNullOrEmpty(model.Password))
            {
                user.PasswordHash = HashPassword(model.Password);
            }

            _context.Users.Update(user);
            _context.SaveChanges();
            return Ok(new { success = true, message = "Driver updated successfully." });
        }

        [HttpDelete("DeleteDriver/{id}")]
        public IActionResult DeleteDriver(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null || user.IsDeleted || user.Role != UserRole.Driver)
            {
                return NotFound(new { success = false, message = "Driver not found." });
            }

            user.IsDeleted = true;
            _context.Users.Update(user);
            _context.SaveChanges();

            return Ok(new { success = true, message = "Driver deleted successfully." });
        }
    }
}