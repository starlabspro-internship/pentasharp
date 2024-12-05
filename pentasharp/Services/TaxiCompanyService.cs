using AutoMapper;
using Microsoft.EntityFrameworkCore;
using pentasharp.Data;
using pentasharp.Models.Entities;
using pentasharp.Models.TaxiRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using pentasharp.Interfaces;

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

        public async Task<List<TaxiCompanyRequest>> GetAllCompaniesAsync()
        {
            var companies = await _context.TaxiCompanies.ToListAsync();
            return _mapper.Map<List<TaxiCompanyRequest>>(companies);
        }

        public async Task<TaxiCompanyRequest> GetCompanyByIdAsync(int id)
        {
            var company = await _context.TaxiCompanies.FindAsync(id);
            if (company == null)
            {
                throw new InvalidOperationException("Company not found");
            }
            return _mapper.Map<TaxiCompanyRequest>(company);
        }

        public async Task<bool> AddCompanyAsync(TaxiCompanyRequest model)
        {
            var company = _mapper.Map<TaxiCompany>(model);
            _context.TaxiCompanies.Add(company);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateCompanyAsync(int id, TaxiCompanyRequest model)
        {
            var company = await _context.TaxiCompanies.FindAsync(id);
            if (company == null)
            {
                throw new InvalidOperationException("Company not found");
            }

            _mapper.Map(model, company);
            company.UpdatedAt = DateTime.UtcNow;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteCompanyAsync(int id)
        {
            var company = await _context.TaxiCompanies.FindAsync(id);
            if (company == null)
            {
                throw new InvalidOperationException("Company not found");
            }

            _context.TaxiCompanies.Remove(company);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<TaxiRequest>> GetTaxisByCompanyAsync(int taxiCompanyId)
        {
            var taxis = await _context.Taxis
                .Where(t => t.TaxiCompanyId == taxiCompanyId)
                .ToListAsync();
            return _mapper.Map<List<TaxiRequest>>(taxis);
        }

        public async Task<bool> AddTaxiAsync(TaxiRequest model)
        {
            var taxi = _mapper.Map<Taxi>(model);
            _context.Taxis.Add(taxi);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateTaxiAsync(int taxiId, TaxiRequest model)
        {
            var taxi = await _context.Taxis.FindAsync(taxiId);
            if (taxi == null)
            {
                throw new InvalidOperationException("Taxi not found");
            }

            _mapper.Map(model, taxi);
            taxi.UpdatedAt = DateTime.UtcNow;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteTaxiAsync(int taxiId)
        {
            var taxi = await _context.Taxis.FindAsync(taxiId);
            if (taxi == null)
            {
                throw new InvalidOperationException("Taxi not found");
            }

            _context.Taxis.Remove(taxi);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
