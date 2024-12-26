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
using pentasharp.Models.Enums;
using pentasharp.Models.Utilities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using pentasharp.ViewModel.Dashboard;

namespace WebApplication1.Controllers
{
    [ServiceFilter(typeof(AdminBaseFilter))]
    [Route("Dashboard")]
    public class DashboardController : Controller
    {
        private readonly IBusCompanyService _busCompanyService;
        private readonly ITaxiCompanyService _taxiCompanyService;
        private readonly IDashboardService _dashboardService;

        public DashboardController(ITaxiCompanyService taxiCompanyService, IBusCompanyService busCompanyService ,IDashboardService dashboardService)
        {
            _taxiCompanyService = taxiCompanyService;
            _busCompanyService = busCompanyService;
            _dashboardService = dashboardService;
        }

        public IActionResult Dashboard()
        {
            var model = _dashboardService.GetDashboardData();
            return View(model);
        }

        [HttpGet("Bus")]
        public IActionResult Bus()
        {
            var busCompanies = _busCompanyService.GetAllBusCompanies();
            var viewModel = new ManageBusCompanyViewModel
            {
                BusCompanies = busCompanies
            };

            return View(viewModel);
        }

        [HttpGet("Taxi")]
        public IActionResult Taxi()
        {
            var taxiCompanies = _taxiCompanyService.GetAllCompanies();

            var viewModel = new ManageTaxiCompanyRequest
            {
                TaxiCompanies = taxiCompanies
            };

            return View(viewModel);
        }
    }
}