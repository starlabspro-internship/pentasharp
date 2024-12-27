using pentasharp.Data;
using pentasharp.Models.Entities;
using pentasharp.Models.TaxiRequest;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using pentasharp.Models.Enums;

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

            return _mapper.Map<List<TaxiCompanyRequest>>(companies ?? new List<TaxiCompany>());
        }

        public List<TaxiCompanyRequest> GetAllCompaniesWithTaxis()
        {
            var companies = _context.TaxiCompanies
                                    .Include(c => c.Taxis)
                                    .ToList();

            if (companies == null || !companies.Any())
            {
                throw new KeyNotFoundException("No taxi companies found.");
            }

            return _mapper.Map<List<TaxiCompanyRequest>>(companies);
        }

        public async Task<TaxiCompany> AddCompanyAsync(TaxiCompanyRequest model)
        {
            var company = _mapper.Map<TaxiCompany>(model);
            _context.TaxiCompanies.Add(company);
            await _context.SaveChangesAsync();
            return company;
        }

        public async Task<bool> AddCompanyAndAssignUserAsync(TaxiCompanyRequest model)
        {
            var company = await AddCompanyAsync(model);
            if (company == null)
                return false;

            if (model.UserId != 0)
            {
                var user = await _context.Users.FindAsync(model.UserId);
                if (user != null)
                {
                    user.CompanyId = company.TaxiCompanyId;
                    user.Role = UserRole.Admin;
                    await _context.SaveChangesAsync();
                }
            }

            return true;
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

        public async Task<bool> EditCompanyAndAssignUserAsync(int id, TaxiCompanyRequest model)
        {
            var result = await EditCompanyAsync(id, model);
            if (!result)
                return false;

            if (model.UserId != 0)
            {
                var user = await _context.Users.FindAsync(model.UserId);
                if (user != null)
                {
                    user.CompanyId = id;
                    user.Role = UserRole.Admin;
                    await _context.SaveChangesAsync();
                }
            }

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

        public List<object> GetUnassignedTaxiCompanyUsers()
        {
            var users = _context.Users
                .Where(user => user.BusinessType == BusinessType.TaxiCompany && user.CompanyId == null)
                .Select(user => new
                {
                    user.UserId,
                    user.FirstName,
                    user.LastName
                })
                .ToList<object>();

            return users;
        }

        public object GetTaxiCompanyUser(int companyId)
        {
            var company = _context.TaxiCompanies
                .Where(tc => tc.TaxiCompanyId == companyId)
                .Select(tc => new
                {
                    User = _context.Users
                        .Where(u => u.CompanyId == companyId)
                        .Where(user => user.BusinessType == BusinessType.TaxiCompany)
                        .Select(u => new
                        {
                            u.UserId,
                            u.FirstName,
                            u.LastName
                        })
                        .FirstOrDefault()
                })
                .FirstOrDefault();

            return company;
        }
    }
}