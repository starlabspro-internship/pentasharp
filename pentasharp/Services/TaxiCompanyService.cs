using pentasharp.Data;
using pentasharp.Models.Entities;
using pentasharp.ViewModel.TaxiModels;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using pentasharp.Models.TaxiRequest;

namespace pentasharp.Services
{
    public class TaxiCompanyService : ITaxiCompanyService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public TaxiCompanyService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<TaxiCompanyRequest> GetAllCompanies()
        {
            var companies = _context.TaxiCompanies.ToList();
            return _mapper.Map<List<TaxiCompanyRequest>>(companies);
        }

        public async Task<TaxiCompany> AddCompanyAsync(TaxiCompanyRequest model)
        {
            var company = _mapper.Map<TaxiCompany>(model);
            _context.TaxiCompanies.Add(company);
            await _context.SaveChangesAsync();
            return company;
        }

        public TaxiCompany GetCompanyById(int id)
        {
            return _context.TaxiCompanies.Find(id);
        }

        public async Task<bool> EditCompanyAsync(int id, TaxiCompanyRequest model)
        {
            var company = await _context.TaxiCompanies.FindAsync(id);
            if (company == null)
                return false;

            _mapper.Map(model, company);
            _context.TaxiCompanies.Update(company);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCompanyAsync(int id)
        {
            var company = await _context.TaxiCompanies
                                         .Include(c => c.Taxis)
                                         .FirstOrDefaultAsync(c => c.TaxiCompanyId == id);
            if (company == null)
                return false;

            company.IsDeleted = true;
            company.UpdatedAt = DateTime.UtcNow;

            foreach (var taxi in company.Taxis)
            {
                taxi.IsDeleted = true;
                taxi.UpdatedAt = DateTime.UtcNow;
            }

            _context.TaxiCompanies.Update(company);
            _context.Taxis.UpdateRange(company.Taxis);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}