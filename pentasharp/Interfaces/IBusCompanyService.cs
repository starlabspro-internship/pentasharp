using pentasharp.ViewModel.Bus;
using System.Threading.Tasks;

namespace pentasharp.Interfaces
{
    public interface IBusCompanyService
    {
        Task<List<BusCompanyViewModel>> GetCompaniesAsync();
        Task<bool> AddCompanyAsync(BusCompanyViewModel model);
        Task<bool> EditCompanyAsync(int id, BusCompanyViewModel model);
        Task<bool> DeleteCompanyAsync(int id);
        Task<object> GetBusCompanyUserAsync(int companyId);
        Task<List<object>> GetBusCompanyUsersAsync();
        Task<object> GetCompanyByUserIdAsync(int userId);
    }
}