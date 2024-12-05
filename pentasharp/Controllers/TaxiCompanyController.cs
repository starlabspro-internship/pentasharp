using Microsoft.AspNetCore.Mvc;
using pentasharp.Interfaces;
using pentasharp.Models.TaxiRequest; 
using System.Threading.Tasks;
using WebApplication1.Filters;

namespace WebApplication1.Controllers
{
    [Route("api/TaxiCompany")]
    [ServiceFilter(typeof(AdminOnlyFilter))]
    public class TaxiCompanyController : Controller
    {
        private readonly ITaxiCompanyService _taxiCompanyService;

        public TaxiCompanyController(ITaxiCompanyService taxiCompanyService)
        {
            _taxiCompanyService = taxiCompanyService;
        }

        [HttpGet("Add")]
        public IActionResult AddTaxiCompany()
        {
            return View();
        }

        [HttpGet("GetCompanies")]
        public async Task<IActionResult> GetCompanies()
        {
            var companies = await _taxiCompanyService.GetAllCompaniesAsync();
            return Ok(companies);
        }

        [HttpPost("AddCompany")]
        public async Task<IActionResult> AddCompany([FromBody] TaxiCompanyRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Invalid data provided." });
            }

            var result = await _taxiCompanyService.AddCompanyAsync(model);
            if (!result)
            {
                return BadRequest(new { success = false, message = "Failed to add company." });
            }

            return Ok(new { success = true, message = "Company added successfully." });
        }

        [HttpPut("EditCompany/{id}")]
        public async Task<IActionResult> EditCompany(int id, [FromBody] TaxiCompanyRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Invalid data provided." });
            }

            try
            {
                var result = await _taxiCompanyService.UpdateCompanyAsync(id, model);
                if (!result)
                {
                    return BadRequest(new { success = false, message = "Failed to update company." });
                }

                return Ok(new { success = true, message = "Company updated successfully." });
            }
            catch (System.InvalidOperationException ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        [HttpDelete("DeleteCompany/{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            try
            {
                var result = await _taxiCompanyService.DeleteCompanyAsync(id);
                if (!result)
                {
                    return BadRequest(new { success = false, message = "Failed to delete company." });
                }

                return Ok(new { success = true, message = "Company deleted successfully." });
            }
            catch (System.InvalidOperationException ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("GetTaxis/{taxiCompanyId}")]
        public async Task<IActionResult> GetTaxis(int taxiCompanyId)
        {
            var taxis = await _taxiCompanyService.GetTaxisByCompanyAsync(taxiCompanyId);
            return Ok(taxis);
        }

        [HttpPost("AddTaxi")]
        public async Task<IActionResult> AddTaxi([FromBody] TaxiRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Invalid data provided." });
            }

            var result = await _taxiCompanyService.AddTaxiAsync(model);
            if (!result)
            {
                return BadRequest(new { success = false, message = "Failed to add taxi." });
            }

            return Ok(new { success = true, message = "Taxi added successfully." });
        }

        [HttpPut("EditTaxi/{id}")]
        public async Task<IActionResult> EditTaxi(int id, [FromBody] TaxiRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Invalid data provided." });
            }

            try
            {
                var result = await _taxiCompanyService.UpdateTaxiAsync(id, model);
                if (!result)
                {
                    return BadRequest(new { success = false, message = "Failed to update taxi." });
                }

                return Ok(new { success = true, message = "Taxi updated successfully." });
            }
            catch (System.InvalidOperationException ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        [HttpDelete("DeleteTaxi/{id}")]
        public async Task<IActionResult> DeleteTaxi(int id)
        {
            try
            {
                var result = await _taxiCompanyService.DeleteTaxiAsync(id);
                if (!result)
                {
                    return BadRequest(new { success = false, message = "Failed to delete taxi." });
                }

                return Ok(new { success = true, message = "Taxi deleted successfully." });
            }
            catch (System.InvalidOperationException ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }
    }
}
