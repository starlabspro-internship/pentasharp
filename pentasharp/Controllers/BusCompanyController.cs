using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pentasharp.Data;
using pentasharp.Models.Entities;
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

        public BusCompanyController(AppDbContext context, IMapper mapper)
        {
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

        [HttpPost("AddCompany")]
        public IActionResult AddCompany([FromBody] BusCompanyViewModel model)
        {
            if (ModelState.IsValid)
            {
                var company = _mapper.Map<BusCompany>(model);
                _context.BusCompanies.Add(company);
                _context.SaveChanges();
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
            var buses = _context.Buses.Include(b => b.BusCompany).ToList();
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
            var bus = _context.Buses.Find(id);
            if (bus == null) return NotFound(new { success = false, message = "Bus not found." });

            _mapper.Map(model, bus);
            _context.SaveChanges();
            return Ok(new { success = true, message = "Bus updated successfully." });
        }

        [HttpDelete("DeleteBus/{id}")]
        public IActionResult DeleteBus(int id)
        {
            var bus = _context.Buses.Find(id);
            if (bus == null) return NotFound(new { success = false, message = "Bus not found." });

            _context.Buses.Remove(bus);
            _context.SaveChanges();
            return Ok(new { success = true, message = "Bus deleted successfully." });
        }
    }
}