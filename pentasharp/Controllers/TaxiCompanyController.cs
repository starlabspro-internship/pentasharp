using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pentasharp.Data;
using pentasharp.Models.Entities;
using pentasharp.ViewModel.Taxi;
using pentasharp.ViewModel.TaxiModels;
using WebApplication1.Filters;
using pentasharp.Models.Enums;

namespace WebApplication1.Controllers
{
    [Route("api/TaxiCompany")]
    [ServiceFilter(typeof(AdminOnlyFilter))]
    public class TaxiCompanyController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public TaxiCompanyController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("GetTaxiStatuses")]
        public IActionResult GetTaxiStatuses()
        {
            var statuses = Enum.GetValues(typeof(TaxiStatus))
                               .Cast<TaxiStatus>()
                               .Select(s => new { Name = s.ToString(), Value = (int)s })
                               .ToList();

            return Ok(statuses);
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

        [HttpPut("EditCompany/{id}")]
        public IActionResult EditCompany(int id, [FromBody] TaxiCompanyViewModel model)
        {
            var company = _context.TaxiCompanies.Find(id);
            if (company == null)
            {
                return NotFound(new { success = false, message = "Company not found." });
            }

            _mapper.Map(model, company);
            _context.SaveChanges();
            return Ok(new { success = true, message = "Company updated successfully." });
        }

        [HttpDelete("DeleteCompany/{id}")]
        public IActionResult DeleteCompany(int id)
        {
            var company = _context.TaxiCompanies.Include(c => c.Taxis).FirstOrDefault(c => c.TaxiCompanyId == id);
            if (company == null)
            {
                return NotFound(new { success = false, message = "Company not found." });
            }

            _context.TaxiCompanies.Remove(company);
            _context.SaveChanges();
            return Ok(new { success = true, message = "Company deleted successfully." });
        }

        [HttpPost("AddTaxi")]
        public IActionResult AddTaxi([FromBody] AddTaxiViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!Enum.TryParse<TaxiStatus>(model.Status, true, out var status))
                {
                    return BadRequest(new { success = false, message = "Invalid status value." });
                }

                var existingTaxi = _context.Taxis.FirstOrDefault(t => t.LicensePlate == model.LicensePlate);
                if (existingTaxi != null)
                {
                    return BadRequest(new { success = false, message = "Taxi with the same License Plate already exists." });
                }

                var taxi = _mapper.Map<Taxi>(model);
                taxi.Status = status;

                _context.Taxis.Add(taxi);
                _context.SaveChanges();
                return Ok(new { success = true, message = "Taxi added successfully." });
            }
            return BadRequest(new { success = false, message = "Invalid data provided." });
        }

        [HttpGet("GetTaxis")]
        public IActionResult GetTaxis()
        {
            var taxis = _context.Taxis.Include(t => t.TaxiCompany).ToList();
            var viewModel = taxis.Select(t => new TaxiViewModel
            {
                TaxiId = t.TaxiId,
                LicensePlate = t.LicensePlate,
                DriverName = t.DriverName,
                Status = t.Status.ToString(),
                CompanyName = t.TaxiCompany.CompanyName
            }).ToList();

            return Ok(viewModel);
        }

        [HttpPut("EditTaxi/{id}")]
        public IActionResult EditTaxi(int id, [FromBody] EditTaxiViewModel model)
        {
            var taxi = _context.Taxis.Find(id);
            if (taxi == null)
            {
                return NotFound(new { success = false, message = "Taxi not found." });
            }

            if (!Enum.TryParse<TaxiStatus>(model.Status, true, out var status))
            {
                return BadRequest(new { success = false, message = "Invalid status value." });
            }

            _mapper.Map(model, taxi);
            taxi.Status = status;

            _context.SaveChanges();
            return Ok(new { success = true, message = "Taxi updated successfully." });
        }

        [HttpDelete("DeleteTaxi/{id}")]
        public IActionResult DeleteTaxi(int id)
        {
            var taxi = _context.Taxis.Find(id);
            if (taxi == null)
            {
                return NotFound(new { success = false, message = "Taxi not found." });
            }

            _context.Taxis.Remove(taxi);
            _context.SaveChanges();
            return Ok(new { success = true, message = "Taxi deleted successfully." });
        }
    }
}
