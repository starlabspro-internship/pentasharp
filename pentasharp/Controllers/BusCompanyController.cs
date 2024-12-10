using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pentasharp.Data;
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
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BusCompanyController(AppDbContext context, IMapper mapper,IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
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
        public IActionResult GetBusCompanyUsers()
        {
            var users = _context.Users
                .Where(user => user.BusinessType == BusinessType.BusCompany)
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

        [HttpPost("AddCompany")]
        public IActionResult AddCompany([FromBody] BusCompanyViewModel model)
        {
            if (ModelState.IsValid)
            {

                var company = _mapper.Map<BusCompany>(model);

                _context.BusCompanies.Add(company);
                _context.SaveChanges();

                var user = _context.Users.FirstOrDefault(u => u.UserId == model.UserId);
                if (user != null)
                {
                    user.CompanyId = company.BusCompanyId; 
                    _context.Users.Update(user);
                    _context.SaveChanges();
                }

                return Ok(new { success = true, message = "Company added successfully." });
            }

            return BadRequest(new { success = false, message = "Invalid data provided." });
        }

        [HttpGet("GetCompanies")]
        public IActionResult GetCompanies()
        {
            var companies = _context.BusCompanies.Include(c => c.Buses).ToList();
            var viewModel = _mapper.Map<List<BusCompanyViewModel>>(companies);
            return Ok(viewModel);
        }

        [HttpPut("EditCompany/{id}")]
        public IActionResult EditCompany(int id, [FromBody] BusCompanyViewModel model)
        {
            var company = _context.BusCompanies.Find(id);
            if (company == null) return NotFound(new { success = false, message = "Company not found." });

            _mapper.Map(model, company);
            _context.SaveChanges();
            return Ok(new { success = true, message = "Company updated successfully." });
        }

        [HttpDelete("DeleteCompany/{id}")]
        public IActionResult DeleteCompany(int id)
        {
            var company = _context.BusCompanies.Include(c => c.Buses)
                                                .FirstOrDefault(c => c.BusCompanyId == id);

            if (company == null)
            {
                return NotFound(new { success = false, message = "Company not found." });
            }

            company.IsDeleted = true;
            company.UpdatedAt = DateTime.UtcNow;

            var users = _context.Users.Where(u => u.CompanyId == id).ToList();
            foreach (var user in users)
            {
                user.CompanyId = null;
                _context.Users.Update(user);
            }

            foreach (var bus in company.Buses)
            {
                bus.IsDeleted = true;
                bus.UpdatedAt = DateTime.UtcNow;
            }

            _context.BusCompanies.Update(company);
            _context.Buses.UpdateRange(company.Buses);

            _context.SaveChanges();

            return Ok(new { success = true, message = "Company and its buses soft deleted successfully." });
        }

        [HttpGet("GetCompany")]
        public IActionResult GetCompany()
        {
            var userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserID");

            if (userId == null)
            {
                return Unauthorized(new { success = false, message = "User not logged in." });
            }

            var company = _context.BusCompanies
                .FirstOrDefault(c => c.UserId == userId);

            if (company == null)
            {
                return NotFound(new { success = false, message = "No company associated with the logged-in user." });
            }

            return Ok(new
            {
                company.BusCompanyId,
                company.CompanyName
            });
        }

        [HttpPost("AddBus")]
        public IActionResult AddBus([FromBody] AddBusViewModel model)
        {

            if (ModelState.IsValid)
            {
                var bus = _mapper.Map<Buses>(model);
                _context.Buses.Add(bus);
                _context.SaveChanges();
                return Ok(new { success = true, message = "Bus added successfully." });
            }

            return BadRequest(new { success = false, message = "Invalid data provided." });
        }

        [HttpGet("GetBuses")]
        public IActionResult GetBuses()
        {
            var userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserID");

            if (userId == null)
            {
                return Unauthorized(new { success = false, message = "User not logged in." });
            }

            var company = _context.BusCompanies.FirstOrDefault(c => c.UserId == userId);
            if (company == null)
            {
                return NotFound(new { success = false, message = "No company associated with the logged-in user." });
            }

            var buses = _context.Buses
                .Where(b => b.BusCompanyId == company.BusCompanyId)
                .Include(b => b.BusCompany)
                .ToList();

            var viewModel = buses.Select(b => new BusViewModel
            {
                BusId = b.BusId,
                BusNumber = b.BusNumber,
                Capacity = b.Capacity,
                BusCompanyId = b.BusCompanyId,
                CompanyName = b.BusCompany.CompanyName
            }).ToList();

            return Ok(viewModel);
        }

        [HttpPut("EditBus/{id}")]
        public IActionResult EditBus(int id, [FromBody] EditBusViewModel model)
        {
            var userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserID");

            if (userId == null)
            {
                return Unauthorized(new { success = false, message = "User not logged in." });
            }

            var bus = _context.Buses.Include(b => b.BusCompany).FirstOrDefault(b => b.BusId == id);
            if (bus == null)
            {
                return NotFound(new { success = false, message = "Bus not found." });
            }

            if (bus.BusCompany.UserId != userId)
            {
                return Unauthorized(new { success = false, message = "You are not authorized to edit this bus." });
            }

            _mapper.Map(model, bus);
            _context.SaveChanges();

            return Ok(new { success = true, message = "Bus updated successfully." });
        }

        [HttpDelete("DeleteBus/{id}")]
        public IActionResult DeleteBus(int id)
        {
            var userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserID");

            if (userId == null)
            {
                return Unauthorized(new { success = false, message = "User not logged in." });
            }

            var bus = _context.Buses.Include(b => b.BusCompany).FirstOrDefault(b => b.BusId == id);
            if (bus == null)
            {
                return NotFound(new { success = false, message = "Bus not found." });
            }

            if (bus.BusCompany.UserId != userId)
            {
                return Unauthorized(new { success = false, message = "You are not authorized to delete this bus." });
            }

            _context.Buses.Remove(bus);
            _context.SaveChanges();

            return Ok(new { success = true, message = "Bus deleted successfully." });
        }
    }
}