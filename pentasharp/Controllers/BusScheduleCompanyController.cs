using AutoMapper;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using pentasharp.Data;
using pentasharp.Mappings;
using pentasharp.Models.Entities;
using pentasharp.ViewModel.Bus;
using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.Intrinsics.X86;

using Microsoft.AspNetCore.Mvc.Filters;
using WebApplication1.Filters;



namespace pentasharp.Controllers
{
    [ServiceFilter(typeof(AdminOnlyFilter))]
    public class BusScheduleCompanyController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        // Constructor to initialize context and AutoMapper
        public BusScheduleCompanyController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult AddBusCompany()
        {
            var busCompanies = _context.BusCompanies.ToList();
            var buses = _context.Buses.ToList();

            var viewModel = new AddBusCompanyViewModel
            {
                BusCompanies = busCompanies,  // Populate the dropdown list with bus companies
                Buses = buses                 // Populate the list of buses
            };

            return View(viewModel);
        }
        // POST: AddBusCompany
        [HttpPost]
        public IActionResult AddBusCompany(BusCompanyViewModel busCompanyViewModel)
        {
            if (ModelState.IsValid)
            {
                // Map the ViewModel to BusCompany entity
                var busCompany = _mapper.Map<BusCompany>(busCompanyViewModel);

                // Save the new bus company to the database
                _context.BusCompanies.Add(busCompany);
                _context.SaveChanges();

                // Redirect to the index or another page
                return RedirectToAction("AddBusCompany");
            }

            return View(busCompanyViewModel); // Return the view if the model is invalid
        } // Edit: Get bus company details for editing
        [HttpGet]
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

        // Edit: Update bus company details
        [HttpPost]
        public IActionResult Edit(int id, BusCompanyViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var busCompany = _context.BusCompanies.FirstOrDefault(b => b.BusCompanyId == id);
                if (busCompany == null)
                {
                    return NotFound();
                }

                // Update the bus company details
                _mapper.Map(viewModel, busCompany);
                _context.SaveChanges();

                return RedirectToAction("AddBusCompany");
            }

            return View(viewModel);
        }
        // Delete: Delete a bus company
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var busCompany = _context.BusCompanies.FirstOrDefault(b => b.BusCompanyId == id);
            if (busCompany == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<BusCompanyViewModel>(busCompany);
            return View("DeleteBusCompany", viewModel);  // Explicitly specify the view name
        }

        // POST: Confirm deletion
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var busCompany = _context.BusCompanies.FirstOrDefault(b => b.BusCompanyId == id);
            if (busCompany == null)
            {
                return NotFound();
            }

            _context.BusCompanies.Remove(busCompany);
            _context.SaveChanges();

            return RedirectToAction("AddBusCompany");  // Redirect to the list or another page after deletion
        }
        [HttpGet]
        public IActionResult AddBus()
        {
            var viewModel = new AddBusViewModel
            {
                BusCompanies = _context.BusCompanies.ToList() // Populate the dropdown
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddBus(AddBusViewModel addBusViewModel)
        {
            if (ModelState.IsValid)
            {
                // Map the ViewModel to the Buses entity
                var bus = _mapper.Map<Buses>(addBusViewModel);

                // Save the new bus to the database
                _context.Buses.Add(bus);
                _context.SaveChanges();

                // Set a success message to display on the same page
                TempData["SuccessMessage"] = "Bus added successfully!";

                // Clear the form by resetting ViewModel properties if needed
                addBusViewModel = new AddBusViewModel();
            }

            // Repopulate the dropdown list of BusCompanies in all cases
            addBusViewModel.BusCompanies = _context.BusCompanies.ToList();

            // Return the same view with the updated model, so the user stays on the page
            return RedirectToAction("AddBusCompany", "BusScheduleCompany");
        }


        [HttpGet]
        public IActionResult EditBus(int id)
        {
            var bus = _context.Buses.Include(b => b.BusCompany).FirstOrDefault(b => b.BusId == id);

            if (bus == null)
            {
                return NotFound();
            }

            // Use AutoMapper to map the entity to the EditBusViewModel
            var model = _mapper.Map<EditBusViewModel>(bus);

            // Populate the dropdown list with bus companies
            model.BusCompanies = _context.BusCompanies.ToList();

            return View(model);  // Make sure to pass EditBusViewModel
        }

        // POST: EditBus[HttpPost]
        [HttpPost]
        public IActionResult EditBus(EditBusViewModel model)
        {
            // Debugging the value of BusCompanyId
            Console.WriteLine($"Selected BusCompanyId: {model.BusCompanyId}");

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
        // GET: DeleteBus
        [HttpGet]
        public IActionResult DeleteBus(int id)
        {
            var bus = _context.Buses.Include(b => b.BusCompany).FirstOrDefault(b => b.BusId == id);
            if (bus == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<AddBusViewModel>(bus); // Map to a ViewModel for display purposes
            return View("DeleteBus", viewModel); // Ensure the view is named "DeleteBus"
        }
        // POST: Confirm deletion of Bus
        [HttpPost]
        public IActionResult DeleteBusConfirmed(int id)
        {
            var bus = _context.Buses.FirstOrDefault(b => b.BusId == id);
            if (bus == null)
            {
                return NotFound();
            }

            _context.Buses.Remove(bus);
            _context.SaveChanges();

            return RedirectToAction("AddBusCompany"); // Redirect after deletion
        }

    }


}

