using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pentasharp.Data;
using pentasharp.Models.Entities;
using pentasharp.ViewModel.TaxiModels;
using WebApplication1.Filters;
using System;
using System.Linq;
using pentasharp.ViewModel.TaxiModels;

namespace pentasharp.Controllers
{
    [Route("api/TaxiCompanyBooking")]
    [ServiceFilter(typeof(AdminOnlyFilter))]
    public class TaxiCompanyBookingController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TaxiCompanyBookingController(AppDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("AddTaxiCompany")]
        public IActionResult AddTaxiCompany()
        {
            var taxiCompanies = _context.TaxiCompanies.ToList();
            var taxis = _context.Taxis.ToList();
            var viewModel = new AddTaxiCompanyViewModel
            {
                TaxiCompanies = taxiCompanies,
                Taxis = taxis
            };
            return View(viewModel);
        }

        [HttpPost("AddTaxiCompany")]
        public IActionResult AddTaxiCompany(TaxiCompanyViewModel taxiCompanyViewModel)
        {
            if (ModelState.IsValid)
            {
                var taxiCompany = _mapper.Map<TaxiCompany>(taxiCompanyViewModel);
                _context.TaxiCompanies.Add(taxiCompany);
                _context.SaveChanges();
                return RedirectToAction("AddTaxiCompany");
            }
            return View(taxiCompanyViewModel);
        }

        [HttpGet("EditTaxiCompany/{id}")]
        public IActionResult EditTaxiCompany(int id)
        {
            var taxiCompany = _context.TaxiCompanies.Find(id);
            if (taxiCompany == null)
            {
                return NotFound();
            }
            var viewModel = _mapper.Map<TaxiCompanyViewModel>(taxiCompany);
            return View(viewModel);
        }

        [HttpPost("EditTaxiCompany/{id}")]
        [ValidateAntiForgeryToken]  
        public IActionResult EditTaxiCompany(int id, TaxiCompanyViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var taxiCompany = _context.TaxiCompanies.Find(id);
            if (taxiCompany == null)
            {
                return NotFound();
            }

            _mapper.Map(viewModel, taxiCompany);
            _context.SaveChanges();
            return RedirectToAction("AddTaxiCompany");  
        }

        [HttpGet("DeleteTaxiCompany/{id}")]
        public IActionResult DeleteTaxiCompany(int id)
        {
            var taxiCompany = _context.TaxiCompanies.FirstOrDefault(tc => tc.TaxiCompanyId == id);
            if (taxiCompany == null)
            {
                return NotFound();
            }
            var viewModel = _mapper.Map<TaxiCompanyViewModel>(taxiCompany);
            return View("DeleteTaxiCompany", viewModel);
        }

        [HttpPost("DeleteTaxiCompanyConfirmed/{id}")]
        public IActionResult DeleteTaxiCompanyConfirmed(int id)
        {
            var taxiCompany = _context.TaxiCompanies.FirstOrDefault(tc => tc.TaxiCompanyId == id);
            if (taxiCompany == null)
            {
                return NotFound();
            }
            _context.TaxiCompanies.Remove(taxiCompany);
            _context.SaveChanges();
            return RedirectToAction("AddTaxiCompany");
        }

        [HttpGet("AddTaxi")]
        public IActionResult AddTaxi()
        {
            var viewModel = new AddTaxiViewModel
            {
                TaxiCompanies = _context.TaxiCompanies.ToList()
            };
            return View(viewModel);
        }

        [HttpPost("AddTaxi")]
        public IActionResult AddTaxi(AddTaxiViewModel addTaxiViewModel)
        {
            if (ModelState.IsValid)
            {
                var taxi = _mapper.Map<Taxi>(addTaxiViewModel);
                _context.Taxis.Add(taxi);
                _context.SaveChanges();
                return RedirectToAction("AddTaxiCompany", "TaxiCompanyBooking");
            }

            addTaxiViewModel.TaxiCompanies = _context.TaxiCompanies.ToList();
            return View(addTaxiViewModel);
        }

        [HttpGet("EditTaxi/{id}")]
        public IActionResult EditTaxi(int id)
        {
            var taxi = _context.Taxis.Include(t => t.TaxiCompany).FirstOrDefault(t => t.TaxiId == id);
            if (taxi == null)
            {
                return NotFound();
            }
            var viewModel = _mapper.Map<EditTaxiViewModel>(taxi);
            viewModel.TaxiCompanies = _context.TaxiCompanies.ToList();
            return View(viewModel);
        }

        [HttpPost("EditTaxi/{id}")]
        [ValidateAntiForgeryToken]  
        public IActionResult EditTaxi(int id, EditTaxiViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.TaxiCompanies = _context.TaxiCompanies.ToList();  
                return View(viewModel);
            }

            var taxi = _context.Taxis.Find(id);
            if (taxi == null)
            {
                return NotFound();
            }

            _mapper.Map(viewModel, taxi);
            _context.SaveChanges();
            return RedirectToAction("AddTaxiCompany");  
        }

        [HttpGet("DeleteTaxi/{id}")]
        public IActionResult DeleteTaxi(int id)
        {
            var taxi = _context.Taxis.Include(t => t.TaxiCompany).FirstOrDefault(t => t.TaxiId == id);
            if (taxi == null)
            {
                return NotFound();
            }
            var viewModel = _mapper.Map<AddTaxiViewModel>(taxi);
            return View("DeleteTaxi", viewModel);
        }

        [HttpPost("DeleteTaxiConfirmed/{id}")]
        public IActionResult DeleteTaxiConfirmed(int id)
        {
            var taxi = _context.Taxis.FirstOrDefault(t => t.TaxiId == id);
            if (taxi == null)
            {
                return NotFound();
            }
            _context.Taxis.Remove(taxi);
            _context.SaveChanges();
            return RedirectToAction("AddTaxiCompany");
        }
    }
}
