using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pentasharp.Data;
using pentasharp.Models.Entities;
using pentasharp.ViewModel.Bus;
using WebApplication1.Filters;
using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;

namespace pentasharp.Controllers
{
    [Route("api/bus-schedule")]
    [ServiceFilter(typeof(AdminOnlyFilter))]
    public class BusScheduleCompanyController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BusScheduleCompanyController(AppDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("add-bus-company")]
        public IActionResult AddBusCompany()
        {
            var busCompanies = _context.BusCompanies.ToList();
            var buses = _context.Buses.ToList();
            var viewModel = new AddBusCompanyViewModel
            {
                BusCompanies = busCompanies,
                Buses = buses
            };
            return View(viewModel);
        }


        [HttpPost("add-bus-company")]
        public IActionResult AddBusCompany(BusCompanyViewModel busCompanyViewModel)
        {
            if (ModelState.IsValid)
            {
                var busCompany = _mapper.Map<BusCompany>(busCompanyViewModel);
                _context.BusCompanies.Add(busCompany);
                _context.SaveChanges();
                return RedirectToAction("AddBusCompany");
            }
            return View(busCompanyViewModel);
        }

        [HttpGet("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var busCompany = _context.BusCompanies.FirstOrDefault(b => b.BusCompanyId == id);
            if (busCompany == null)
            {
                return NotFound();
            }
            var viewModel = _mapper.Map<BusCompanyViewModel>(busCompany);
            return View(viewModel);
        }

        [HttpPost("edit/{id}")]
        public IActionResult Edit(int id, BusCompanyViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var busCompany = _context.BusCompanies.FirstOrDefault(b => b.BusCompanyId == id);
                if (busCompany == null)
                {
                    return NotFound();
                }
                _mapper.Map(viewModel, busCompany);
                _context.SaveChanges();
                return RedirectToAction("AddBusCompany");
            }
            return View(viewModel);
        }


        [HttpGet("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var busCompany = _context.BusCompanies.FirstOrDefault(b => b.BusCompanyId == id);
            if (busCompany == null)
            {
                return NotFound();
            }
            var viewModel = _mapper.Map<BusCompanyViewModel>(busCompany);
            return View("DeleteBusCompany", viewModel);
        }

        [HttpPost("delete-confirmed/{id}")]
        public IActionResult DeleteConfirmed(int id)
        {
            var busCompany = _context.BusCompanies.FirstOrDefault(b => b.BusCompanyId == id);
            if (busCompany == null)
            {
                return NotFound();
            }
            _context.BusCompanies.Remove(busCompany);
            _context.SaveChanges();
            return RedirectToAction("AddBusCompany");
        }

        [HttpGet("add-bus")]

        public IActionResult AddBus()
        {
            var viewModel = new AddBusViewModel
            {
                BusCompanies = _context.BusCompanies.ToList() 
            };
            return View(viewModel);
        }
        [HttpPost("add-bus")]
        public IActionResult AddBus(AddBusViewModel addBusViewModel)
        {
            if (ModelState.IsValid)
            {
                var bus = _mapper.Map<Buses>(addBusViewModel);

                _context.Buses.Add(bus);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Bus added successfully!";

                addBusViewModel = new AddBusViewModel();
            }

            addBusViewModel.BusCompanies = _context.BusCompanies.ToList();
            return RedirectToAction("AddBusCompany", "BusScheduleCompany");
        }

        [HttpGet("edit-bus/{id}")]
        public IActionResult EditBusModel(int id)
        {
            var bus = _context.Buses.Include(b => b.BusCompany).FirstOrDefault(b => b.BusId == id);
            if (bus == null)
            {
                return NotFound();
            }
            var model = _mapper.Map<EditBusViewModel>(bus);
            model.BusCompanies = _context.BusCompanies.ToList();
            return View(model);
        }

        [HttpPost("edit-bus/{id}")]
        public IActionResult EditBusModel(EditBusViewModel model)
        {
            if (ModelState.IsValid)
            {
                var bus = _context.Buses.FirstOrDefault(b => b.BusId == model.BusId);
                if (bus == null)
                {
                    return NotFound();
                }
                _mapper.Map(model, bus);
                bus.UpdatedAt = DateTime.UtcNow;
                _context.SaveChanges();
                return RedirectToAction("AddBusCompany");
            }
            model.BusCompanies = _context.BusCompanies.ToList();
            return View(model);
        }

        [HttpGet("delete-bus/{id}")]
        public IActionResult DeleteBus(int id)
        {
            var bus = _context.Buses.Include(b => b.BusCompany).FirstOrDefault(b => b.BusId == id);
            if (bus == null)
            {
                return NotFound();
            }
            var viewModel = _mapper.Map<AddBusViewModel>(bus);
            return View("DeleteBus", viewModel);
        }

        [HttpPost("delete-bus-confirmed/{id}")]
        public IActionResult DeleteBusConfirmed(int id)
        {
            var bus = _context.Buses.FirstOrDefault(b => b.BusId == id);
            if (bus == null)
            {
                return NotFound();
            }
            _context.Buses.Remove(bus);
            _context.SaveChanges();
            return RedirectToAction("AddBusCompany");
        }
    }
}