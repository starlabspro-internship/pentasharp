using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;
using WebApplication1.Filters;
using AutoMapper;
using pentasharp.Data;
using pentasharp.Interfaces;
using pentasharp.ViewModel.Bus;
using pentasharp.Models.TaxiRequest;
using pentasharp.Services;

namespace WebApplication1.Controllers
{
    [ServiceFilter(typeof(AdminBaseFilter))]
    [Route("Dashboard")]
    public class DashboardController : Controller
    {

        private readonly ITaxiCompanyService _taxiCompanyService;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public DashboardController( AppDbContext context, IMapper mapper,ITaxiCompanyService taxiCompanyService)
        {
            _taxiCompanyService = taxiCompanyService;
            _context = context;
            _mapper = mapper;
        }
        public IActionResult Dashboard()
        {
            return View();
        }

        [HttpGet("Bus")]
        public IActionResult Bus()
        {
            var companies = _context.BusCompanies.ToList();

            var viewModel = new ManageBusCompanyViewModel
            {
                BusCompanies = _mapper.Map<List<BusCompanyViewModel>>(companies),
            };
            return View(viewModel);
        }

        [HttpGet("Taxi")]
        public IActionResult Taxi()
        {
            var companies = _taxiCompanyService.GetAllCompanies();
            var viewModel = new ManageTaxiCompanyRequest
            {
                TaxiCompanies = companies,
            };
            return View(viewModel);
        }
    }
}