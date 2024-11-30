using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pentasharp.Data;
using pentasharp.Models.Entities;
using pentasharp.ViewModel.Taxi;
using pentasharp.ViewModel.TaxiModels; 
using WebApplication1.Filters;

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
        public IActionResult AddTaxi([FromBody] AddTaxiViewModel model)
        {
            if (ModelState.IsValid)
            {
                var taxi = _mapper.Map<Taxi>(model);
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

            _mapper.Map(model, taxi);
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
